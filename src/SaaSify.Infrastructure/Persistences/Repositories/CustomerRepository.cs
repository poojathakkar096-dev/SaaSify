using Dapper;
using SaaSify.Application.Interface;
using SaaSify.Domain.Entity;
using System.Data;

namespace SaaSify.Infrastructure.Persistences.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly DapperContext _context;

        public CustomerRepository(DapperContext context)
        {
            _context = context;
        }

        public async Task<Customer> SaveAsync(Customer customer)
        {
            using var connection = _context.CreateConnection();

            var parameters = new DynamicParameters();
            parameters.Add("@Id", customer.Id);
            parameters.Add("@TenantId", customer.TenantId);
            parameters.Add("@Name", customer.Name);
            parameters.Add("@Phone", customer.Phone);
            parameters.Add("@Email", customer.Email);
            parameters.Add("@Address", customer.Address);

            return await connection.QuerySingleAsync<Customer>(
                "sp_SaveCustomer",
                parameters,
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task<IEnumerable<Customer>> GetAllByTenantAsync(Guid tenantId)
        {
            using var connection = _context.CreateConnection();

            var parameters = new DynamicParameters();
            parameters.Add("@TenantId", tenantId);

            return await connection.QueryAsync<Customer>(
                "sp_GetCustomersByTenant",
                parameters,
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task<Customer?> GetByIdAsync(Guid id, Guid tenantId)
        {
            using var connection = _context.CreateConnection();

            var parameters = new DynamicParameters();
            parameters.Add("@Id", id);
            parameters.Add("@TenantId", tenantId);

            return await connection.QuerySingleOrDefaultAsync<Customer>(
                "sp_GetCustomerById",
                parameters,
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task<int> CountByTenantAsync(Guid tenantId)
        {
            using var connection = _context.CreateConnection();

            var parameters = new DynamicParameters();
            parameters.Add("@TenantId", tenantId);

            return await connection.QuerySingleAsync<int>(
                "sp_GetCustomerCountByTenant",
                parameters,
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task<bool> DeleteAsync(Guid id, Guid tenantId)
        {
            using var connection = _context.CreateConnection();

            var parameters = new DynamicParameters();
            parameters.Add("@Id", id);
            parameters.Add("@TenantId", tenantId);

            var rowsAffected = await connection.QuerySingleAsync<int>(
                "sp_DeleteCustomer",
                parameters,
                commandType: CommandType.StoredProcedure
            );

            return rowsAffected > 0;
        }
    }
}
