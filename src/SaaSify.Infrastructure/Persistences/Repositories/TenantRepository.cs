using Dapper;
using SaaSify.Application.Interface;
using SaaSify.Domain.Entity;
using System.Data;

namespace SaaSify.Infrastructure.Persistences.Repositories
{
    public class TenantRepository : ITenantRepository
    {
        private readonly DapperContext _context;

        public TenantRepository(DapperContext context)
        {
            _context = context;
        }

        public async Task<Tenant> CreateAsync(Tenant tenant)
        {
            using var connection = _context.CreateConnection();

            var parameters = new DynamicParameters();
            parameters.Add("@Id", tenant.Id);
            parameters.Add("@Name", tenant.Name);
            parameters.Add("@Email", tenant.Email);
            parameters.Add("@SubscriptionPlan", tenant.SubscriptionPlan);

            return await connection.QuerySingleAsync<Tenant>(
                "sp_CreateTenant",
                parameters,
                commandType: CommandType.StoredProcedure
            );
        }

    }
}
