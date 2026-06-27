using SaaSify.Application.Interface;
using SaaSify.Application.Interface.Services;
using SaaSify.Domain.Entity;
using System.Security.Cryptography;
using System.Text;
using static SaaSify.Application.DTOs.Auth;

namespace SaaSify.Infrastructure.Services
{
    public class AuthService : IAuthService
    {
        private readonly ITenantRepository _tenantRepository;
        private readonly IUserRepository _userRepository;
        private readonly IJwtTokenService _jwtTokenService;

        public AuthService(
            ITenantRepository tenantRepository,
            IUserRepository userRepository,
            IJwtTokenService jwtTokenService)
        {
            _tenantRepository = tenantRepository;
            _userRepository = userRepository;
            _jwtTokenService = jwtTokenService;
        }

        public async Task<AuthResponse> SignupAsync(SignupRequest request)
        {
            // Check if user already exists
            var existingUser = await _userRepository.GetByEmailAsync(request.Email);
            if (existingUser != null)
                throw new InvalidOperationException("A user with this email already exists.");

            // Create tenant
            var tenant = new Tenant
            {
                Id = Guid.NewGuid(),
                Name = request.BusinessName,
                Email = request.Email,
                SubscriptionPlan = "Free",
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };

            await _tenantRepository.CreateAsync(tenant);

            // Create admin user
            var user = new User
            {
                Id = Guid.NewGuid(),
                TenantId = tenant.Id,
                Email = request.Email,
                PasswordHash = HashPassword(request.Password),
                FullName = request.FullName,
                Role = "Admin",
                CreatedAt = DateTime.UtcNow
            };

            await _userRepository.CreateAsync(user);

            // Generate JWT
            var token = _jwtTokenService.GenerateToken(user);

            return new AuthResponse
            {
                Token = token,
                UserId = user.Id,
                TenantId = user.TenantId,
                Email = user.Email,
                FullName = user.FullName,
                Role = user.Role
            };
        }

        public async Task<AuthResponse?> LoginAsync(LoginRequest request)
        {
            var user = await _userRepository.GetByEmailAsync(request.Email);
            if (user == null) return null;

            // Verify password
            if (user.PasswordHash != HashPassword(request.Password))
                return null;

            // Generate JWT
            var token = _jwtTokenService.GenerateToken(user);

            return new AuthResponse
            {
                Token = token,
                UserId = user.Id,
                TenantId = user.TenantId,
                Email = user.Email,
                FullName = user.FullName,
                Role = user.Role
            };
        }

        private static string HashPassword(string password)
        {
            var bytes = Encoding.UTF8.GetBytes(password);
            var hash = SHA256.HashData(bytes);
            return Convert.ToBase64String(hash);
        }
    }
}
