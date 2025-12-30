using CloudReady.Application.Interfaces;
using System.Security.Claims;

namespace CloudReady.API.Middleware
{
    public class TenantResolutionMiddleware
    {
        private readonly RequestDelegate _next;

        public TenantResolutionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, ITenantProvider tenantProvider)
        {
            if (!context.Request.Headers.TryGetValue("X-Tenant-Code", out var tenantCode))
            {
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                await context.Response.WriteAsync("X-Tenant-Code header is required");
                return;
            }

            tenantProvider.SetTenant(tenantCode!);

            // 🔐 Extract role from JWT (if authenticated)
            if (context.User.Identity?.IsAuthenticated == true)
            {
                var role = context.User.FindFirst(ClaimTypes.Role)?.Value;
                tenantProvider.SetIsAdmin(role == "Admin" || role == "Owner");
            }

            await _next(context);
        }
    }
}
