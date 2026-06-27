namespace SaaSify.Domain.Entity
{
    public class User : BaseEntity, ITenantEntity
    {
        public Guid TenantId { get; set; }
        public string Email { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string Role { get; set; } = "User";  // Admin or User

        // Navigation property
        public Tenant? Tenant { get; set; }
    }
}
