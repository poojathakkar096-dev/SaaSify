namespace SaaSify.Application.DTOs.Subscription
{
    public class UpgradeResponse
    {
        public Guid SubscriptionId { get; set; }
        public string Plan { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;

        // TODO: Phase 2.2 - Will contain Stripe/Razorpay payment intent client_secret
        public string? PaymentIntentClientSecret { get; set; }
    }
}
