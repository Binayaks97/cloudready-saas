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
        public string Email { get; set; } = null!;
        public string PasswordHash { get; set; } = null!;
        public string Role { get; set; } = "User";
        public bool IsActive { get; set; } = true;
    }
}
