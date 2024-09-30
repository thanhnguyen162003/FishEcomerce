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
        private readonly ICustomerRepository _customerRepository;
        private readonly ISupplierRepository _supplierRepository;
        private readonly string _jwtSecretKey;

        public AuthService(ICustomerRepository customerRepository, ISupplierRepository supplierRepository, IConfiguration configuration)
        {
            _customerRepository = customerRepository;
            _supplierRepository = supplierRepository;

            // Lấy SecretKey từ appsettings.json
            _jwtSecretKey = configuration["JwtSettings:SecretKey"];

            // Kiểm tra xem SecretKey có null hoặc trống không
            if (string.IsNullOrEmpty(_jwtSecretKey))
            {
                throw new ArgumentNullException(nameof(_jwtSecretKey), "JWT Secret Key cannot be null or empty.");
            }
        }

        public async Task<bool> RegisterCustomer(string email, string password, string name)
        {
            var existingCustomer = await _customerRepository.GetByEmailAsync(email);
            if (existingCustomer != null) return false;

            var newCustomer = new Customer
            {
                Id = Guid.NewGuid(),
                Email = email,
                Password = HashPassword(password),
                Name = name,
                RegistrationDate = DateOnly.FromDateTime(DateTime.UtcNow)
            };

            await _customerRepository.AddAsync(newCustomer);
            return true;
        }

        public async Task<bool> RegisterSupplier(string username, string password, string companyName)
        {
            var existingSupplier = await _supplierRepository.GetByUsernameAsync(username);
            if (existingSupplier != null) return false;

            var newSupplier = new Supplier
            {
                Id = Guid.NewGuid(),
                Username = username,
                Password = HashPassword(password),
                CompanyName = companyName
            };

            await _supplierRepository.AddAsync(newSupplier);
            return true;
        }

        public async Task<string?> LoginCustomer(string email, string password)
        {
            var customer = await _customerRepository.GetByEmailAsync(email);
            if (customer == null || !VerifyPassword(customer.Password, password)) return null;

            return GenerateJwtToken(customer.Email, "Customer");
        }

        public async Task<string?> LoginSupplier(string username, string password)
        {
            var supplier = await _supplierRepository.GetByUsernameAsync(username);
            if (supplier == null || !VerifyPassword(supplier.Password, password)) return null;

            return GenerateJwtToken(supplier.Username, "Supplier");
        }

        private string GenerateJwtToken(string identifier, string role)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtSecretKey);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Name, identifier),
                    new Claim(ClaimTypes.Role, role)
                }),
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
