namespace SaaSify.Domain.Constants;

public static class AppConstants
{
    public static class SubscriptionStatus
    {
        public const string Active = "Active";
        public const string Cancelled = "Cancelled";
        public const string Expired = "Expired";
        public const string Pending = "Pending";
    }

    public static class PaymentStatus
    {
        public const string Pending = "Pending";
        public const string Success = "Success";
        public const string Failed = "Failed";
        public const string Refunded = "Refunded";
    }

    public static class PaymentGateway
    {
        public const string Stripe = "Stripe";
        public const string Razorpay = "Razorpay";
    }
}
