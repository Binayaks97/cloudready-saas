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
        public string Code { get; set; } = default!;
        public string Name { get; set; } = default!;
        public string Plan { get; set; } = "Free";
        public bool IsActive { get; set; } = true;
    }
}
