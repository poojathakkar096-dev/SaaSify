
using SaaSify.Domain.Entity;

namespace SaaSify.Application.Interface.Services
{
    public interface ISubscriptionService
    {
        IEnumerable<PlanInfo> GetPlans();
        Task<SubscriptionResponse> SaveAsync(SubscriptionRequest request);
        Task<SubscriptionResponse?> GetCurrentAsync();
        Task<IEnumerable<SubscriptionResponse>> GetHistoryAsync();
        Task<bool> CancelAsync();
    }
}
