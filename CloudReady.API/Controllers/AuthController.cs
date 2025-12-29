using CloudReady.Application.DTOs.Auth;
using Microsoft.AspNetCore.Mvc;

namespace CloudReady.API.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        [HttpPost("register")]
        [ProducesResponseType(typeof(AuthResponse), StatusCodes.Status200OK)]
        public ActionResult<AuthResponse> Register([FromBody] RegisterRequest request)
        {
            return Ok(new AuthResponse
            {
                Token = "dummy-token",
                ExpiresAt = DateTime.UtcNow.AddHours(1)
            });
        }

        [HttpPost("login")]
        [ProducesResponseType(typeof(AuthResponse), StatusCodes.Status200OK)]
        public ActionResult<AuthResponse> Login([FromBody] LoginRequest request)
        {
            return Ok(new AuthResponse
            {
                Token = "dummy-token",
                ExpiresAt = DateTime.UtcNow.AddHours(1)
            });
        }
    }
}
