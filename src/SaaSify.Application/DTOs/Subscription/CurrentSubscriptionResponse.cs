namespace SaaSify.Application.DTOs.Subscription
{
    public class CurrentSubscriptionResponse
    {
        public Guid Id { get; set; }
        public string Plan { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        // Plan limits
        public int MaxCustomers { get; set; }

        // Current usage
        public int CurrentCustomerCount { get; set; }
    }
}
