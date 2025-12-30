using CloudReady.Application.DTOs.Auth;
using CloudReady.Application.Interfaces;
using CloudReady.Domain.Entities;
using CloudReady.Domain.Securities;
using CloudReady.Infrastructure.Persistence;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace CloudReady.API.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        [HttpPost("register")]
        public async Task<IActionResult> Register(
            RegisterRequest request,
            AppDbContext db,
            IPasswordHasher hasher,
            IJwtTokenService jwt,
            ITenantProvider tenantProvider)
        {
            var tenantCode = tenantProvider.GetTenantCode();

            var user = new User
            {
                Id = Guid.NewGuid(),
                Email = request.Email,
                PasswordHash = hasher.Hash(request.Password),
                Role = Roles.User,                 // ✅ DEFAULT ROLE
                TenantId = Guid.Empty,
                TenantCode = tenantCode,
                CreatedOn = DateTime.UtcNow
            };

            db.Users.Add(user);
            await db.SaveChangesAsync();

            var token = jwt.GenerateToken(user);

            return Ok(token);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(
            LoginRequest request,
            AppDbContext db,
            IPasswordHasher hasher,
            IJwtTokenService jwt,
            ITenantProvider tenantProvider)
        {
            var tenantCode = tenantProvider.GetTenantCode();

            var user = await db.Users
                .FirstOrDefaultAsync(u =>
                    u.Email == request.Email &&
                    u.TenantCode == tenantCode);

            if (user == null || !hasher.Verify(request.Password, user.PasswordHash))
                return Unauthorized();

            var token = jwt.GenerateToken(user);

            return Ok(token);
        }

        [Authorize]
        [HttpGet("secure")]
        public IActionResult Secure()
        {
            return Ok(new
            {
                Message = "You are authenticated",
                Tenant = User.FindFirst("tenantCode")?.Value,
                User = User.FindFirst(ClaimTypes.Email)?.Value
            });
        }
    }
}
