using CloudReady.Application.Interfaces;

namespace CloudReady.Infrastructure.Tenancy
{
    public class TenantProvider : ITenantProvider
    {
        private string? _tenantCode;
        private bool _isAdmin;

        public void SetTenant(string tenantCode)
        {
            _tenantCode = tenantCode;
        }

        public string GetTenantCode()
        {
            if (string.IsNullOrWhiteSpace(_tenantCode))
                throw new Exception("Tenant not resolved");

            return _tenantCode;
        }

        public void SetIsAdmin(bool isAdmin)
        {
            _isAdmin = isAdmin;
        }

        public bool IsAdmin()
        {
            return _isAdmin;
        }
    }
}
