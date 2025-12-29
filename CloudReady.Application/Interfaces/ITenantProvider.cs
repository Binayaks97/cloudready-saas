using System;
using System.Collections.Generic;
using System.Text;

namespace CloudReady.Application.Interfaces
{
    public interface ITenantProvider
    {
        Guid GetTenantId();
    }
}
