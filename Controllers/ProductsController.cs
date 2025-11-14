using Ecommerce.Dtos;
using Ecommerce.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using System.Threading.Tasks;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly IProductService _productService;
    private readonly IWebHostEnvironment _env;

    public ProductsController(IProductService productService, IWebHostEnvironment env)
    {
        _productService = productService;
        _env = env;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll() => Ok(await _productService.GetAllProductsAsync());

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var product = await _productService.GetProductByIdAsync(id);
        if (product == null) return NotFound();
        return Ok(product);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] ProductCreateDto dto)
    {
        var created = await _productService.CreateProductAsync(dto, _env);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] ProductCreateDto dto)
    {
        if (!await _productService.UpdateProductAsync(id, dto, _env)) return NotFound();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        if (!await _productService.DeleteProductAsync(id)) return NotFound();
        return NoContent();
    }
    //sp

    [HttpGet("from-sp")]
    public async Task<IActionResult> GetAllProductsFromSP()
    {
        var products = await _productService.GetAllProductsFromSPAsync();
        return Ok(products);
    }
}
