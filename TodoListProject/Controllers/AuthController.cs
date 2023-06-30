using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TodoListProject.Models;
using BCrypt.Net;// Install-Package BCrypt.Net-Next

namespace TodoListProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly JwtConfiguration jwtConfiguration;
        PasswordHashing passwordHashing=new PasswordHashing();
        DbContextClasse dbContact = new DbContextClasse();

        public AuthController(JwtConfiguration jwtConfiguration)
        {
            this.jwtConfiguration = jwtConfiguration;
        }

     
        [HttpPost("signUp")]
        public IActionResult SignUp([FromBody] Users user)
        {
            Users test=jwtConfiguration.VerifyEmail(user.Email);
            if (test != null)
            {
                return BadRequest("Email already exist");
            }
            else
            {
                user.Password=passwordHashing.HashPassword(user.Password);
                dbContact.Users.Add(user);
                dbContact.SaveChanges();
                return Ok("User Created");
            }
        }


        [AllowAnonymous]
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

                return Ok(new { Token = token, UserDeatails = jwtConfiguration.getUser(user.email, user.password) });
            }
        }
    }

    public class User
    {
        public string? email { get; set; }
        public string? password { get; set; }
    }

    public class UserSignUp
    {
        public string? Name { get; set; }
        public string? email { get; set; }
        public string? password { get; set; }
    }
}
