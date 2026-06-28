using SaaSify.Domain.Constants;
using SaaSify.Domain.Enums;

namespace SaaSify.Domain.Entity
{
    public class Subscription : BaseEntity, ITenantEntity
    {
        public Guid TenantId { get; set; }
        public Plan PlanId { get; set; }
        public string Status { get; set; } = AppConstants.SubscriptionStatus.Pending;
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }

    public class SubscriptionRequest
    {
        public Plan PlanId { get; set; }
    }

    public class SubscriptionResponse
    {
        public Guid Id { get; set; }
        
        public string? PlanName { get; set; }
        public Plan PlanId { get; set; }
        public string Status { get; set; } = string.Empty;
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }

    public class PlanInfo
    {
        public int PlanId { get; set; }
        public Plan Name { get; set; }
        public decimal Price { get; set; }
        public string Currency { get; set; } = "INR";
        public int MaxCustomers { get; set; }
        public List<string> Features { get; set; } = new();
    }
}
