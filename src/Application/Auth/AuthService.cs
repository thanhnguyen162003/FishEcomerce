using Domain.Entites;
using Infrastructure.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Application.Common.UoW;
using Microsoft.Extensions.Logging;

namespace Application.Auth
{
    public interface IAuthService
    {
        Task<bool> RegisterCustomer(string email, string password, string name);
        Task<bool> RegisterSupplier(string username, string password, string companyName);
        Task<string?> LoginCustomer(string email, string password);
        Task<string?> LoginSupplier(string username, string password);
    }

    public class AuthService : IAuthService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly string _jwtSecretKey;
        private readonly IConfiguration _configuration;
        private readonly ILogger<AuthService> _logger;

        public AuthService(IUnitOfWork unitOfWork, IConfiguration configuration, ILogger<AuthService> logger)
        {
            _unitOfWork = unitOfWork;
            _configuration = configuration;
            _logger = logger;
            // Lấy SecretKey từ appsettings.json
            _jwtSecretKey = configuration["JwtSettings:SecretKey"];

            // Kiểm tra xem SecretKey có null hoặc trống không
            if (string.IsNullOrEmpty(_jwtSecretKey))
            {
                throw new ArgumentNullException(nameof(_jwtSecretKey), "JWT Secret Key cannot be null or empty.");
            }
        }

        //public async Task<bool> RegisterCustomer(string email, string password, string name)
        //{
        //    var existingCustomer = await _unitOfWork.CustomerRepository.GetByEmailAsync(email);
        //    if (existingCustomer != null) return false;

        //    var newCustomer = new Customer
        //    {
        //        Id = Guid.NewGuid(),
        //        Email = email,
        //        Password = HashPassword(password),
        //        Name = name,
        //        RegistrationDate = DateOnly.FromDateTime(DateTime.UtcNow)
        //    };

        //    await _unitOfWork.CustomerRepository.AddAsync(newCustomer);
        //    return true;
        //}

        private bool IsValidPassword(string password)
        {
            // Mật khẩu phải có ít nhất 1 chữ cái viết thường, 1 chữ viết hoa và 1 ký tự đặc biệt
            var hasUpperCase = new System.Text.RegularExpressions.Regex(@"[A-Z]+");
            var hasLowerCase = new System.Text.RegularExpressions.Regex(@"[a-z]+");
            var hasSpecialChar = new System.Text.RegularExpressions.Regex(@"[#$%!@&^*]+");

            return hasUpperCase.IsMatch(password) && hasLowerCase.IsMatch(password) && hasSpecialChar.IsMatch(password);
        }


        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }


        public async Task<bool> RegisterCustomer(string email, string password, string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("Fullname is required.");
            }

            // Validate email format
            if (!IsValidEmail(email))
            {
                throw new ArgumentException("Invalid email format.");
            }

            // Kiểm tra độ phức tạp của mật khẩu
            if (!IsValidPassword(password))
            {
                throw new ArgumentException("Password must contain at least one uppercase letter, one lowercase letter, and one special character (#$%!@&^*).");
            }

            var existingCustomer = await _unitOfWork.CustomerRepository.GetByEmailAsync(email);
            if (existingCustomer != null)
            {
                throw new ArgumentException("Customer with this email already exists.");
            }

            var newCustomer = new Customer
            {
                Id = Guid.NewGuid(),
                Email = email,
                Password = HashPassword(password),
                Name = name,
                RegistrationDate = DateOnly.FromDateTime(DateTime.UtcNow)
            };

            await _unitOfWork.CustomerRepository.AddAsync(newCustomer);
            return true;
        }


        //public async Task<bool> RegisterSupplier(string username, string password, string companyName)
        //{
        //    var existingSupplier = await _unitOfWork.SupplierRepository.GetByUsernameAsync(username);
        //    if (existingSupplier != null) return false;

        //    var newSupplier = new Supplier
        //    {
        //        Id = Guid.NewGuid(),
        //        Username = username,
        //        Password = HashPassword(password),
        //        CompanyName = companyName
        //    };

        //    await _unitOfWork.SupplierRepository.AddAsync(newSupplier);
        //    return true;
        //}

        public async Task<bool> RegisterSupplier(string username, string password, string companyName)
        {
            if (string.IsNullOrWhiteSpace(username) || username.Length < 3 || username.Length > 50)
            {
                throw new ArgumentException("Username must be between 3 and 50 characters.");
            }

            // Kiểm tra độ phức tạp của mật khẩu
            if (!IsValidPassword(password))
            {
                throw new ArgumentException("Password must contain at least one uppercase letter, one lowercase letter, and one special character (#$%!@&^*).");
            }

            var existingSupplier = await _unitOfWork.SupplierRepository.GetByUsernameAsync(username);
            if (existingSupplier != null)
            {
                throw new ArgumentException("Supplier with this username already exists.");
            }

            var newSupplier = new Supplier
            {
                Id = Guid.NewGuid(),
                Username = username,
                Password = HashPassword(password),
                CompanyName = companyName
            };

            await _unitOfWork.SupplierRepository.AddAsync(newSupplier);
            return true;
        }


        public async Task<string?> LoginCustomer(string email, string password)
        {
            var customer = await _unitOfWork.CustomerRepository.GetByEmailAsync(email);
            if (customer == null)
            {
                _logger.LogError("Customer not found for email: {email}", email);
                return null;
            }
            if (customer.IsDeleted == true)
            {
                _logger.LogError("Customer with email: {email} is deleted.", email);
                return null;
            }
            if (!VerifyPassword(customer.Password, password))
            {
                _logger.LogError("Password incorrect for customer with email: {email}", email);
                return null;
            }

            return GenerateJwtToken(customer.Id.ToString(), customer.Email, customer.Name, "Customer");
        }




        public async Task<string?> LoginSupplier(string username, string password)
        {
            var supplier = await _unitOfWork.SupplierRepository.GetByUsernameAsync(username);
            if (supplier == null || !VerifyPassword(supplier.Password, password)) return null;

            return GenerateJwtToken(supplier.Id.ToString(), supplier.Username, "", "Supplier");
        }

        private string GenerateJwtToken(string userId, string identifier, string fullname, string role)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtSecretKey);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Name, identifier),
                    new Claim(ClaimTypes.Role, role),
                    new Claim("Fullname", fullname),
                    new Claim("UserId", userId)
                }),
                Issuer = _configuration["JwtSettings:Issuer"],
                Audience = _configuration["JwtSettings:Audience"],
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        private string HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(bytes);
        }

        private bool VerifyPassword(string hashedPassword, string password)
        {
            return hashedPassword == HashPassword(password);
        }


    }
}
