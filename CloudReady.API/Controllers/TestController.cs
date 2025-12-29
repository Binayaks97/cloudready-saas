using Microsoft.AspNetCore.Mvc;

namespace CloudReady.API.Controllers
{
    [ApiController]
    [Route("api/test")]
    public class TestController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            return Ok("Tenant resolved successfully");
        }
    }
}
