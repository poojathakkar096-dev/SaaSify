using SaaSify.Domain.Entity;

namespace SaaSify.Application.Interface.Repositories
{
    public interface ISubscriptionRepository
    {
        Task<Subscription> SaveAsync(Subscription subscription);
        Task<Subscription?> GetCurrentAsync(Guid tenantId);
        Task<IEnumerable<Subscription>> GetHistoryAsync(Guid tenantId);
        Task<bool> CancelAsync(Guid tenantId);
    }
}
