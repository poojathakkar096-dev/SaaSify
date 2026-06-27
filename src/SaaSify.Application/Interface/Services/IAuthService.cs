using static SaaSify.Application.DTOs.Auth;

namespace SaaSify.Application.Interface.Services
{
    public interface IAuthService
    {
        Task<AuthResponse> SignupAsync(SignupRequest request);
        Task<AuthResponse?> LoginAsync(LoginRequest request);
    }
}
