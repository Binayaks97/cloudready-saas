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

        public async Task InvokeAsync(HttpContext context, TenantProvider tenantProvider)
        {
            if (!context.Request.Headers.TryGetValue("X-Tenant-Id", out var tenantIdValue) ||
                !Guid.TryParse(tenantIdValue, out var tenantId))
            {
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                await context.Response.WriteAsync("Tenant Id is missing or invalid.");
                return;
            }

            tenantProvider.SetTenant(tenantId);
            await _next(context);
        }
    }
}
