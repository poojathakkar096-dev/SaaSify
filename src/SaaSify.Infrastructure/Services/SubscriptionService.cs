
using SaaSify.Application.Interface.Repositories;
using SaaSify.Application.Interface.Services;
using SaaSify.Domain.Constants;
using SaaSify.Domain.Entity;
using SaaSify.Domain.Enums;

namespace SaaSify.Infrastructure.Services;

public class SubscriptionService : ISubscriptionService
{
    private readonly ISubscriptionRepository _subscriptionRepository;
    private readonly ITenantContext _tenantContext;

    public SubscriptionService(
        ISubscriptionRepository subscriptionRepository,
        ITenantContext tenantContext)
    {
        _subscriptionRepository = subscriptionRepository;
        _tenantContext = tenantContext;
    }

    public IEnumerable<PlanInfo> GetPlans()
    {
        return new List<PlanInfo>
        {
            new()
            {
                PlanId = 1,
                Name = Plan.Free,
                Price = 0,
                Currency = "INR",
                MaxCustomers = 10,
                Features = new List<string>
                {
                    "Up to 10 customers",
                    "Basic CRM features",
                    "Email support"
                }
            },
            new()
            {
                PlanId = 2,
                Name = Plan.Pro,
                Price = 999,
                Currency = "INR",
                MaxCustomers = 100,
                Features = new List<string>
                {
                    "Up to 100 customers",
                    "Advanced CRM features",
                    "Priority support",
                    "Reports & analytics"
                }
            },
            new()
            {
                PlanId = 3,
                Name = Plan.Premium,
                Price = 2499,
                Currency = "INR",
                MaxCustomers = -1, // -1 means unlimited
                Features = new List<string>
                {
                    "Unlimited customers",
                    "All Pro features",
                    "AI Assistant",
                    "API access",
                    "Dedicated support"
                }
            }
        };
    }

    public async Task<SubscriptionResponse> SaveAsync(SubscriptionRequest request)
    {
        // Validate plan name
        if (request.PlanId == 0)
            throw new InvalidOperationException($"Invalid plan: {request.PlanId}");

        var subscription = new Subscription
        {
            Id = Guid.NewGuid(),
            TenantId = _tenantContext.TenantId,
            PlanId = request.PlanId,
            Status = AppConstants.SubscriptionStatus.Active,
            StartDate = DateTime.UtcNow,
            CreatedAt = DateTime.UtcNow
        };

        var saved = await _subscriptionRepository.SaveAsync(subscription);
        return MapToResponse(saved);
    }

    public async Task<SubscriptionResponse?> GetCurrentAsync()
    {
        var subscription = await _subscriptionRepository.GetCurrentAsync(_tenantContext.TenantId);
        return subscription == null ? null : MapToResponse(subscription);
    }

    public async Task<IEnumerable<SubscriptionResponse>> GetHistoryAsync()
    {
        var subscriptions = await _subscriptionRepository.GetHistoryAsync(_tenantContext.TenantId);
        return subscriptions.Select(MapToResponse);
    }

    public async Task<bool> CancelAsync()
    {
        return await _subscriptionRepository.CancelAsync(_tenantContext.TenantId);
    }

    // ============================================================
    // Private Helpers
    // ============================================================

    private static bool IsValidPlan(string plan)
    {
        return plan == Plan.Free.ToString()
            || plan == Plan.Pro.ToString()
            || plan == Plan.Premium.ToString();
    }

    private static SubscriptionResponse MapToResponse(Subscription subscription)
    {
        return new SubscriptionResponse
        {
            Id = subscription.Id,

            PlanId = subscription.PlanId,
            PlanName = Enum.TryParse<Plan>(subscription.PlanId.ToString(), out var plan) ? plan.ToString() : "Unknown",
            Status = subscription.Status,
            StartDate = subscription.StartDate,
            EndDate = subscription.EndDate,
            CreatedAt = subscription.CreatedAt,
            UpdatedAt = subscription.UpdatedAt
        };
    }
}