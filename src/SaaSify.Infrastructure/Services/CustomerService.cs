using SaaSify.Application.DTOs;
using SaaSify.Application.Interface;
using SaaSify.Application.Interface.Services;
using SaaSify.Domain.Entity;

namespace SaaSify.Infrastructure.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly ITenantContext _tenantContext;

        public CustomerService(
            ICustomerRepository customerRepository,
            ITenantContext tenantContext)
        {
            _customerRepository = customerRepository;
            _tenantContext = tenantContext;
        }

        public async Task<CustomerResponse> SaveAsync(CustomerRequest request)
        {
            var customer = new Customer
            {
                Id = request.Id ?? Guid.NewGuid(),       
                TenantId = _tenantContext.TenantId,
                Name = request.Name,
                Phone = request.Phone,
                Email = request.Email,
                Address = request.Address
            };

            var saved = await _customerRepository.SaveAsync(customer);
            return MapToResponse(saved);
        }


        public async Task<IEnumerable<CustomerResponse>> GetAllAsync()
        {
            var customers = await _customerRepository.GetAllByTenantAsync(_tenantContext.TenantId);
            return customers.Select(MapToResponse);
        }

        public async Task<CustomerResponse?> GetByIdAsync(Guid id)
        {
            var customer = await _customerRepository.GetByIdAsync(id, _tenantContext.TenantId);
            return customer == null ? null : MapToResponse(customer);
        }

       

        public async Task<bool> DeleteAsync(Guid id)
        {
            return await _customerRepository.DeleteAsync(id, _tenantContext.TenantId);
        }

        // Private helper to map entity → DTO
        private static CustomerResponse MapToResponse(Customer customer)
        {
            return new CustomerResponse
            {
                Id = customer.Id,
                Name = customer.Name,
                Phone = customer.Phone,
                Email = customer.Email,
                Address = customer.Address,
                CreatedAt = customer.CreatedAt,
                UpdatedAt = customer.UpdatedAt
            };
        }

    }
}
