using CloudReady.Application.Interfaces;
using CloudReady.Infrastructure.Tenancy;

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
            await _next(context);
        }
    }
}
