using SaaSify.Domain.Entity;

namespace SaaSify.Application.Interface
{
    public interface IUserRepository
    {
        Task<User?> GetByEmailAsync(string email);
        Task<User> CreateAsync(User user);

    }
}
