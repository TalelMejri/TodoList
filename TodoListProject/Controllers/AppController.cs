using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TodoListProject.Models;

namespace TodoListProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AppController : ControllerBase
    {
        DbContextClasse dbContact = new DbContextClasse();

      
        [HttpGet]
        public List<Todos> GetAll()
        {
            List<Todos> todos = this.dbContact.Todos.ToList();
            return todos;
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
                return Ok("Deleted with success");
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
    }
}
