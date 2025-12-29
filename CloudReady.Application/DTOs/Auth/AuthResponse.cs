using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudReady.Application.DTOs.Auth
{
    public class AuthResponse
    {
        public string Token { get; set; } = default!;
        public DateTime ExpiresAt { get; set; }
    }
}
