using CloudReady.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudReady.Domain.Entities
{
    public class User : BaseEntity
    {
        public string Email { get; set; } = default!;
        public string PasswordHash { get; set; } = default!;
        public string TenantCode { get; set; } = default!;
        public string? Role { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
