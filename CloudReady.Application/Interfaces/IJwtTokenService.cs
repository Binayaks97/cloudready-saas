using CloudReady.Application.DTOs.Auth;
using CloudReady.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudReady.Application.Interfaces
{
    public interface IJwtTokenService
    {
        AuthResponse GenerateToken(User user);
    }
}
