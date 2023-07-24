using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TodoListProject.Models;
using System.Linq;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using static System.Net.Mime.MediaTypeNames;

namespace TodoListProject.Controllers
{
   
    [Route("api/[controller]")]
    [ApiController]
    public class AppController : ControllerBase
    {
        DbContextClasse dbContact = new DbContextClasse();
        private readonly IHttpContextAccessor _httpContextAccessor;

        private readonly RequestDelegate _next;

      
        public AppController(IHttpContextAccessor httpContextAccessor, RequestDelegate next)
        {
            _next = next;
            _httpContextAccessor = httpContextAccessor;
        }
       /* private void AttachUserToContext(HttpContext context, string token)
        {
            try
            {
                var jwtSettings = context.RequestServices.GetRequiredService<IOptions<JwtSettings>>();
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(jwtSettings.Value.SecretKey);

                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtSettings.Value.Issuer,
                    ValidAudience = jwtSettings.Value.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(key)
                }, out SecurityToken validatedToken);

                var jwtToken = (JwtSecurityToken)validatedToken;
                var userId = jwtToken.Claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value;

                // Set the user ID in the HttpContext for future access in controllers or services
                context.Items["UserId"] = userId;
            }
            catch
            {
                // Handle any exception if token validation fails
            }
        }*/

        [HttpGet]
        public string GetAll(HttpContext context)
        {
            //var userId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            //var todosForUser = this.dbContact.Todos.Where(t => t.UserId == userId).ToList();
          
            var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

            /*  if (token != null)
              {
                  AttachUserToContext(context, token);
              }
            */
            return "gg";
            //return userId;
           /* List<Todos> todos = this.dbContact.Todos.ToList();
            return todos;*/
        }

       
        [HttpGet("{id}")]
        public IActionResult GetTodos(int id)
        {
            Todos todo = this.dbContact.Todos.Find(id);
            if (todo == null)
            {
                return NotFound($"todo {id} not found");
            }
            else
            {
                return Ok(todo);
            }
        }

      
        [HttpPost]
        public IActionResult Post(Todos todos)
        {
            try
            {
                todos.IsCompleted = false;
                todos.UserId = 6;
              /*  string userid= User.Identity.Name;
                todos.UserId = userid!=null ? userid : 0;*/
                dbContact.Add(todos);
                dbContact.SaveChanges();
                return Ok("Todos Created");
            }
            catch(Exception e)
            {
                return BadRequest(e.Message);
            }  
        }

       
        [HttpPut("{id}")]
        public IActionResult Put(int id,Todos todos) {
            var todosUpdates = dbContact.Todos.Find(id);
            if (todosUpdates == null)
            {
                return NotFound();
            }
            else
            {
                todosUpdates.DueDate = todos.DueDate;
                todosUpdates.Title = todos.Title;
                todosUpdates.Description = todos.Description;
                dbContact.SaveChanges();
                return Ok("Todos Updated");
            }
        }

       
        [HttpDelete("{id}")]
        public IActionResult DeleteTodos(int id)
        {
            Todos todo = dbContact.Todos.Where(e => e.Id == id).FirstOrDefault();
            if (todo != null)
            {
                dbContact.Remove(todo);
                dbContact.SaveChanges();
                return Ok(new { message="Deleted with success"});
            }
            else
            {
                return NotFound($"todo {id} not found");
            }
        }

    
        [HttpGet("countTodos")]
        public IActionResult CountTodos()
        {
            List<Todos> count = dbContact.Todos.ToList();
            return Ok(count.Count());
        }

        [HttpPut("changerStat")]
        public IActionResult ChangerSatat(int id)
        {
            var todosUpdates = dbContact.Todos.Find(id);
            if (todosUpdates == null)
            {
                return NotFound();
            }
            else
            {
                todosUpdates.IsCompleted = !todosUpdates.IsCompleted;
                dbContact.SaveChanges();
                return Ok(new { message = "Todos Updated" });
            }
        }

    }
}
