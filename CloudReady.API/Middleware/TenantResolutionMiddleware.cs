using CloudReady.Application.Interfaces;

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
            // 1. Read tenant from header
            if (!context.Request.Headers.TryGetValue("X-Tenant-Code", out var tenantCode))
            {
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                await context.Response.WriteAsync("X-Tenant-Code header is required");
                return;
            }

            var requestedTenant = tenantCode.ToString();

            // 2. If user is authenticated, validate tenant against JWT
            if (context.User.Identity?.IsAuthenticated == true)
            {
                var tokenTenant = context.User.FindFirst("tenantCode")?.Value;

                if (string.IsNullOrWhiteSpace(tokenTenant))
                {
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    await context.Response.WriteAsync("Tenant claim missing in token");
                    return;
                }

                if (!string.Equals(tokenTenant, requestedTenant, StringComparison.OrdinalIgnoreCase))
                {
                    context.Response.StatusCode = StatusCodes.Status403Forbidden;
                    await context.Response.WriteAsync("Tenant mismatch");
                    return;
                }
            }

            // 3. Set tenant in provider (used by EF Core filters)
            tenantProvider.SetTenant(requestedTenant);

            await _next(context);
        }
    }
}
