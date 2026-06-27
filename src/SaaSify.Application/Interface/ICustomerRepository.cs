using SaaSify.Domain.Entity;

namespace SaaSify.Application.Interface
{
    public interface ICustomerRepository
    {
        Task<Customer> SaveAsync(Customer customer);
        Task<IEnumerable<Customer>> GetAllByTenantAsync(Guid tenantId);
        Task<Customer?> GetByIdAsync(Guid id, Guid tenantId);
        Task<bool> DeleteAsync(Guid id, Guid tenantId);
    }
}
