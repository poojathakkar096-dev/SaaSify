using Dapper;
using SaaSify.Application.Interface;
using SaaSify.Domain.Entity;
using System.Data;

namespace SaaSify.Infrastructure.Persistences.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly DapperContext _context;

        public UserRepository(DapperContext context)
        {
            _context = context;
        }

        public async Task<User?> GetByEmailAsync(string email)
        {
            using var connection = _context.CreateConnection();

            var parameters = new DynamicParameters();
            parameters.Add("@Email", email);

            return await connection.QuerySingleOrDefaultAsync<User>(
                "sp_GetUserByEmail",
                parameters,
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task<User> CreateAsync(User user)
        {
            using var connection = _context.CreateConnection();

            var parameters = new DynamicParameters();
            parameters.Add("@Id", user.Id);
            parameters.Add("@TenantId", user.TenantId);
            parameters.Add("@Email", user.Email);
            parameters.Add("@PasswordHash", user.PasswordHash);
            parameters.Add("@FullName", user.FullName);
            parameters.Add("@Role", user.Role);

            return await connection.QuerySingleAsync<User>(
                "sp_CreateUser",
                parameters,
                commandType: CommandType.StoredProcedure
            );
        }

    }
}
