using CloudReady.Domain.Securities;
using CloudReady.Infrastructure.Persistence;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CloudReady.API.Controllers
{
    [Authorize(Policy = "RequireAdmin")]
    [ApiController]
    [Route("api/admin")]
    public class AdminController : ControllerBase
    {
        private readonly AppDbContext _db;

        public AdminController(AppDbContext db)
        {
            _db = db;
        }

        [HttpGet("dashboard")]
        public IActionResult Dashboard()
        {
            return Ok("Admin access granted");
        }

        // ✅ NEW: Change user role
        [HttpPut("users/{userId}/role")]
        public async Task<IActionResult> UpdateUserRole(
            Guid userId,
            [FromBody] string role)
        {
            // Validate role
            if (!new[] { Roles.User, Roles.Admin, Roles.Owner }.Contains(role))
                return BadRequest("Invalid role");

            var user = await _db.Users.FirstOrDefaultAsync(u => u.Id == userId);
            if (user == null)
                return NotFound("User not found");

            user.Role = role;
            await _db.SaveChangesAsync();

            return Ok($"Role updated to {role}");
        }
    }
}
