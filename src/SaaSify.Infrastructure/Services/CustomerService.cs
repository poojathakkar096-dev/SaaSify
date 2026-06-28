using SaaSify.Application.DTOs;
using SaaSify.Application.Interface;
using SaaSify.Application.Interface.Services;
using SaaSify.Domain.Entity;
using SaaSify.Domain.Enums;

namespace SaaSify.Infrastructure.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly ITenantContext _tenantContext;
        private readonly ISubscriptionService _subscriptionService;

        public CustomerService(
            ICustomerRepository customerRepository,
            ISubscriptionService subscriptionService,
            ITenantContext tenantContext)
        {
            _customerRepository = customerRepository;
            _subscriptionService = subscriptionService;
            _tenantContext = tenantContext;
        }

        public async Task<CustomerResponse> SaveAsync(CustomerRequest request)
        {
            if (request.Id == null || request.Id == Guid.Empty)
            {
                await CheckPlanLimitAsync();
            }
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

        private async Task CheckPlanLimitAsync()
        {
            // Get current subscription
            var subscription = await _subscriptionService.GetCurrentAsync();
            if (subscription == null)
                throw new InvalidOperationException("No active subscription found.");

            // Get max customers allowed for this plan
            var maxCustomers = GetMaxCustomersForPlan(subscription.PlanId);

            // -1 means unlimited (Premium plan)
            if (maxCustomers == -1) return;

            // Count current customers
            var currentCount = await _customerRepository.CountByTenantAsync(_tenantContext.TenantId);

            if (currentCount >= maxCustomers)
            {
                throw new InvalidOperationException(
                    $"Customer limit reached ({maxCustomers}) for your {subscription.PlanId} plan. " +
                    $"Please upgrade to add more customers."
                );
            }
        }

        private static int GetMaxCustomersForPlan(Plan planId)
        {
            return planId switch
            {
                Plan.Free => 10,
                Plan.Pro => 100,
                Plan.Premium => -1,    // Unlimited
                _ => 10
            };
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
