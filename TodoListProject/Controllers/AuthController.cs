using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace TodoListProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly JwtConfiguration jwtConfiguration;

        public AuthController(JwtConfiguration jwtConfiguration)
        {
            this.jwtConfiguration = jwtConfiguration;
        }

        [AllowAnonymous]
        [Authorize]
        [HttpPost("authUser")]
        public IActionResult Login([FromBody]User user)
        {
            var token=jwtConfiguration.authenticate(user.email, user.password);
            if (token == null)
            {
                return Unauthorized();
            }
            else
            {
                return Ok(token);
            }
        }
    }

    public class User
    {
        public string? email { get; set; }
        public string? password { get; set; }
    }
}
