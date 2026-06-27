using SaaSify.Application.Interface.Services;

namespace SaaSify.Infrastructure.Services
{
    public class TenantContext : ITenantContext
    {
        public Guid TenantId { get; private set; }
        public Guid UserId { get; private set; }
        public bool IsAuthenticated => TenantId != Guid.Empty;

        public void SetTenant(Guid tenantId, Guid userId)
        {
            TenantId = tenantId;
            UserId = userId;
        }
    }
}
