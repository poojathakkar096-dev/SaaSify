using SaaSify.Application.Interface.Services;
using System.Security.Claims;

namespace SaaSify.Api.Middleware
{
    public class TenantMiddleware
    {
        private readonly RequestDelegate _next;

        public TenantMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, ITenantContext tenantContext)
        {
            // Check if user is authenticated
            if (context.User.Identity?.IsAuthenticated == true)
            {
                // Extract TenantId from JWT claims
                var tenantIdClaim = context.User.FindFirst("TenantId")?.Value;
                var userIdClaim = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                               ?? context.User.FindFirst("sub")?.Value;

                if (!string.IsNullOrEmpty(tenantIdClaim) && !string.IsNullOrEmpty(userIdClaim))
                {
                    if (Guid.TryParse(tenantIdClaim, out var tenantId) &&
                        Guid.TryParse(userIdClaim, out var userId))
                    {
                        // Set the tenant context for this request
                        tenantContext.SetTenant(tenantId, userId);
                    }
                }
            }

            await _next(context);
        }
    }
}
