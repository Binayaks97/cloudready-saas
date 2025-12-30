using System;
using System.Collections.Generic;
using System.Text;

namespace CloudReady.Application.Interfaces
{
    public interface ITenantProvider
    {
        void SetTenant(string tenantCode);
        string GetTenantCode();

        void SetIsAdmin(bool isAdmin);
        bool IsAdmin();
    }
}
