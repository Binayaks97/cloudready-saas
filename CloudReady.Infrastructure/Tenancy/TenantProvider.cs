using CloudReady.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace CloudReady.Infrastructure.Tenancy
{
    public class TenantProvider : ITenantProvider
    {
        private Guid _tenantId;

        public void SetTenant(Guid tenantId)
        {
            _tenantId = tenantId;
        }

        public Guid GetTenantId()
        {
            if (_tenantId == Guid.Empty)
                throw new Exception("Tenant not resolved");

            return _tenantId;
        }
    }
}
