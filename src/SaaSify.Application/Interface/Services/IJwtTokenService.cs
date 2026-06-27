using SaaSify.Domain.Entity;

namespace SaaSify.Application.Interface.Services
{
    public interface IJwtTokenService
    {
        string GenerateToken(User user);

    }
}
