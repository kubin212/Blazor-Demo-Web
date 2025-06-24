using API.Models;
using API.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientsController : ControllerBase
    {
        private readonly ApplicationDbContext context;

        public ClientsController(ApplicationDbContext context)
        {
            this.context = context;
            // Constructor logic can be added here if needed
        }

        [HttpGet]
        public List<Client> GetClients()
        {
            return context.Clients.OrderBy(c => c.Id).ToList();
        }

        [HttpGet("{id}")]
        public IActionResult GetClient(int id)
        {
            var client = context.Clients.Find(id);
            if (client == null)
            {
                return NotFound();
            }
            return Ok(client);
        }

        [HttpPost]
        public IActionResult CreateClient([FromBody] ClientDto clientDto)
        {
            var otherClient = context.Clients.FirstOrDefault(c => c.Email == clientDto.Email);
            if (otherClient != null)
            {
                ModelState.AddModelError("Email", "A client with this email already exists.");
                var validation = new ValidationProblemDetails(ModelState);    
                return BadRequest(validation);
            }

            var client = new Client
            {
                Name = clientDto.Name,
                Email = clientDto.Email,
                PhoneNumber = clientDto.PhoneNumber ?? "",
                Address = clientDto.Address ?? "",
                Status = clientDto.Status,
                CreatedAt = DateTime.UtcNow
            };

            context.Clients.Add(client);
            context.SaveChanges();

            return Ok(client);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateClient(int id, [FromBody] ClientDto clientDto)
        {
            var client = context.Clients.Find(id);
            if (client == null)
                return NotFound();
            
            var otherClient = context.Clients.FirstOrDefault(c => c.Email == clientDto.Email && c.Id != id);
            if (otherClient != null)
            {
                ModelState.AddModelError("Email", "A client with this email already exists.");
                var validation = new ValidationProblemDetails(ModelState);    
                return BadRequest(validation);
            }

            client.Name = clientDto.Name;
            client.Email = clientDto.Email;
            client.PhoneNumber = clientDto.PhoneNumber ?? "";
            client.Address = clientDto.Address ?? "";
            client.Status = clientDto.Status;
            context.SaveChanges();

            return Ok(client);
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteClient(int id)
        {
            var client = context.Clients.Find(id);
            if (client == null)
                return NotFound();

            context.Clients.Remove(client);
            context.SaveChanges();

            return Ok();
        }
    }
}
