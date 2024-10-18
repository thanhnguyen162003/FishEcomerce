using Domain.Entites;
using Infrastructure.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Application.Common.Models;
using Application.Common.UoW;
using Application.Common.Utils;
using Microsoft.Extensions.Logging;

namespace Application.Auth
{
    public interface IAuthService
    {
        Task<ResponseModel> RegisterCustomer(string username, string password, string name);
        Task<ResponseModel> LoginCustomer(string email, string password);
        Task<ResponseModel> LoginStaff(string username, string password);
    }

    public class AuthService : IAuthService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly string _jwtSecretKey;
        private readonly IConfiguration _configuration;

        public AuthService(IUnitOfWork unitOfWork, IConfiguration configuration)
        {
            _unitOfWork = unitOfWork;
            _configuration = configuration;
            _jwtSecretKey = configuration["JwtSettings:SecretKey"];
            
            if (string.IsNullOrEmpty(_jwtSecretKey))
            {
                throw new NullReferenceException("JWT Secret Key is null or empty.");
            }
        }

        public async Task<ResponseModel> RegisterCustomer(string username, string password, string name)
        {
            var existingCustomer = await _unitOfWork.CustomerRepository.CheckUserByUsernameRegister(username);
            
            if (existingCustomer)
            {
                return new ResponseModel(HttpStatusCode.BadRequest, "Account has been exits!");
            }

            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(password);
            var customer = new Customer
            {
                Id = new UuidV7().Value,
                Username = username,
                Password = hashedPassword,
                Name = name,
                RegistrationDate = DateOnly.FromDateTime(DateTime.UtcNow)
            };

            await _unitOfWork.BeginTransactionAsync();

            try
            {
                await _unitOfWork.CustomerRepository.AddAsync(customer);
                await _unitOfWork.SaveChangesAsync();
                await _unitOfWork.CommitTransactionAsync();
                return new ResponseModel(HttpStatusCode.OK, "Customer added successfully!");
            }
            catch (Exception e)
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw;
            }
        }

        public async Task<ResponseModel> LoginCustomer(string email, string password)
        {
            var customer = await _unitOfWork.CustomerRepository.GetByUsernameAsync(email);
            
            if (customer is null)
            {
                return new ResponseModel(HttpStatusCode.NotFound, "Your account not found.");
            }
            
            if (customer.DeletedAt != null)
            {
                return new ResponseModel(HttpStatusCode.Forbidden, "Your account has been suspended.");
            }
            
            if (!BCrypt.Net.BCrypt.Verify(password, customer.Password))
            {
                return new ResponseModel(HttpStatusCode.BadRequest, "Wrong password!");
            }

            var token = GenerateJwtToken(customer.Id.ToString(), customer.Username, customer.Name, "Customer");
            return new ResponseModel(HttpStatusCode.OK, token);
        }

        public async Task<ResponseModel> LoginStaff(string username, string password)
        {
            var staff = await _unitOfWork.StaffRepository.GetByUsernameAsync(username);

            if (staff is null)
            {
                return new ResponseModel(HttpStatusCode.NotFound, "Your account not found.");
            }

            if (staff.DeletedAt != null)
            {
                return new ResponseModel(HttpStatusCode.Forbidden, "Your account has been suspended.");
            }

            if (!BCrypt.Net.BCrypt.Verify(password, staff.Password))
            {
                return new ResponseModel(HttpStatusCode.BadRequest, "Wrong password!");
            }
            
            var role = (bool)staff.IsAdmin! ? "Admin" : "Staff";
            
            var token = GenerateJwtToken(staff.Id.ToString(), staff.Username, staff.FullName, role);
            return new ResponseModel(HttpStatusCode.OK, token);
        }

        private string GenerateJwtToken(string userId, string identifier, string? fullname, string role)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtSecretKey);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim("UserId", userId),
                    new Claim(ClaimTypes.Name, identifier),
                    new Claim("Fullname", fullname ?? ""),
                    new Claim(ClaimTypes.Role, role)
                }),
                Issuer = _configuration["JwtSettings:Issuer"],
                Audience = _configuration["JwtSettings:Audience"],
                Expires = DateTime.UtcNow.AddHours(2),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
