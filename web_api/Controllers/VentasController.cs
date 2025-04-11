using Microsoft.AspNetCore.Mvc;
using web_api.Models;
using web_api.Services;

namespace web_api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VentasController(IVentasService ventasService) : ControllerBase
    {
        // GET: api/ventas
        [HttpGet]
        public async Task<ActionResult<List<Ventas>>> GetAllAsync()
        {
            var ventas = await ventasService.GetAllAsync();
            return Ok(ventas);
        }

        // GET: api/ventas/5
        [HttpGet("{id}", Name = "GetVentasById")]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            var ventas = await ventasService.GetByIdAsync(id);

            return Ok(ventas);
        }

        // POST: api/ventas
        [HttpPost]
        public async Task<IActionResult> AddAsync(Ventas ventas)
        {
            var id = await ventasService.AddAsync(ventas);

            // Devuelve 201 Created con la URL
            return CreatedAtRoute("GetVentasById", new { id }, ventas);
        }

        // PUT: api/ventas/5
        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateAsync(int id, [FromBody] Ventas? ventas)
        {
            if (ventas == null || id != ventas.IdVenta) 
                return BadRequest("Datos inválidos.");

            ventas.IdVenta = id;
            var updated = await ventasService.UpdateAsync(ventas);
            if (!updated) 
                return NotFound();

            return NoContent();
        }

        // DELETE: api/ventas/5
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var result = await ventasService.DeleteAsync(id);
            if (!result)
                return NotFound();
                
            return NoContent();
        }
    }
}