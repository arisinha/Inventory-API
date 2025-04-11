using Microsoft.AspNetCore.Mvc;
using web_api.Models;
using web_api.Repository;

namespace web_api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProductsController(IProductRepository productRepository) : ControllerBase
{
    // api/products
    [HttpPost]
    public async Task<IActionResult> AddProduct([FromBody] Product? product)
    {
        if (product == null) return BadRequest("Datos inválidos.");

        var newProductId = await productRepository.AddAsync(product);
        return newProductId == 0 ? StatusCode(500, "Error al insertar el producto.") : CreatedAtAction(nameof(GetProductById), new { id = newProductId }, product);
    }

    // api/products
    [HttpGet]
    public async Task<ActionResult<List<Product>>> GetAllProducts()
    {
        var products = await productRepository.GetAllAsync();
        return Ok(products);
    }

    //  api/products/{id}
    [HttpGet("{id:int}")]
    public async Task<ActionResult<Product>> GetProductById(int id)
    {
        var product = await productRepository.GetByIdAsync(id);
        if (product == null) return NotFound();

        return Ok(product);
    }

    // PUT: api/products/{id}
    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateProduct(int id, [FromBody] Product? product)
    {
        if (product == null || id != product.Id) return BadRequest("Datos inválidos.");

        var updated = await productRepository.UpdateAsync(product);
        if (!updated) return NotFound();

        return NoContent();
    }

    // DELETE: api/products/{id}
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteProduct(int id)
    {
        var deleted = await productRepository.DeleteAsync(id);
        if (!deleted) return NotFound();

        return NoContent();
    }
}