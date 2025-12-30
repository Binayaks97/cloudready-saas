using CloudReady.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace CloudReady.Infrastructure.Tenancy
{
    public class TenantProvider : ITenantProvider
    {
        private string? _tenantCode;

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
    }
}
