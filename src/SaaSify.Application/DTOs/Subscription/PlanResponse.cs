namespace SaaSify.Application.DTOs.Subscription
{
    public class PlanResponse
    {
        public string Name { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int MaxCustomers { get; set; }
        public List<string> Features { get; set; } = new();
    }
}
