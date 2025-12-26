using CloudReady.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudReady.Domain.Entities
{
    public class Tenant : BaseEntity
    {
        public string Name { get; set; } = null!;
        public string Subdomain { get; set; } = null!;
        public string Plan { get; set; } = "Free";
        public bool IsActive { get; set; } = true;
    }
}
