namespace SaaSify.Application.Interface.Services
{
    public interface ITenantContext
    {
        Guid TenantId { get; }
        Guid UserId { get; }
        bool IsAuthenticated { get; }
        void SetTenant(Guid tenantId, Guid userId);
    }
}
