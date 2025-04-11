using Microsoft.AspNetCore.Mvc;
using web_api.Models;
using web_api.Services;

namespace web_api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClientesController(IClientesService clientesService) : ControllerBase
    {
        // POST: api/clientes
        [HttpPost]
        public async Task<IActionResult> AddAsync([FromBody] Clientes? cliente)
        {
            if (cliente == null) return BadRequest("Datos inválidos.");

            var id = await clientesService.AddAsync(cliente);

            return CreatedAtRoute("GetClienteById", new { id }, cliente);
        }

        // GET: api/clientes
        [HttpGet]
        public async Task<ActionResult<List<Clientes>>> GetAllAsync()
        {
            var clientes = await clientesService.GetAllAsync();
            return Ok(clientes);
        }

        // GET: api/clientes/{id}
        [HttpGet("{id:int}", Name = "GetClienteById")]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            var cliente = await clientesService.GetByIdAsync(id);

            return Ok(cliente);
        }

        // PUT: api/clientes/{id}
        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateCliente(int id, [FromBody] Clientes? cliente)
        {
            if (cliente == null || id != cliente.IdCliente) return BadRequest("Datos inválidos.");

            var updated = await clientesService.UpdateAsync(cliente);
            if (!updated) return NotFound();

            return NoContent();
        }
        
        // DELETE: api/clientes/{id}
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteCliente(int id)
        {
            var deleted = await clientesService.DeleteAsync(id);
            if (!deleted) return NotFound();

            return NoContent();
        }
    }
}