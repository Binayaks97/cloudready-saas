using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudReady.Domain.Common
{
    public abstract class BaseEntity
    {
        public Guid Id { get; set; }
        public Guid TenantId { get; set; }
        public DateTime CreatedOn { get; set; } = DateTime.UtcNow;
    }
}
