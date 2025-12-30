using CloudReady.Application.DTOs.Auth;
using CloudReady.Application.Interfaces;
using CloudReady.Domain.Entities;
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
        public async Task<ActionResult<AuthResponse>> Register([FromBody] RegisterRequest request, [FromServices] AppDbContext db, [FromServices] IPasswordHasher hasher, [FromServices] IJwtTokenService jwt, [FromServices] ITenantProvider tenantProvider)
        {
            var tenantCode = tenantProvider.GetTenantCode();

            var user = new User
            {
                Email = request.Email,
                PasswordHash = hasher.Hash(request.Password),
                TenantCode = tenantCode
            };

            db.Users.Add(user);
            await db.SaveChangesAsync();

            return Ok(new AuthResponse
            {
                Token = jwt.GenerateToken(user, tenantCode),
                ExpiresAt = DateTime.UtcNow.AddMinutes(60)
            });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequest request, AppDbContext db, IPasswordHasher hasher, IJwtTokenService jwt, ITenantProvider tenantProvider)
        {
            var tenantCode = tenantProvider.GetTenantCode();

            var user = await db.Users.FirstOrDefaultAsync(u =>
                u.Email == request.Email &&
                u.TenantCode == tenantCode);

            if (user == null)
                return Unauthorized("Invalid credentials");

            if (!hasher.Verify(request.Password, user.PasswordHash))
                return Unauthorized("Invalid credentials");

            var token = jwt.GenerateToken(user, tenantCode);

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
