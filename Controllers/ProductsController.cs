using Ecommerce.Dtos;
using Ecommerce.Interfaces;
using Ecommerce.Models;
using Microsoft.AspNetCore.Mvc;

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
    public async Task<IActionResult> GetAll()
    {
        var products = await _productService.GetAllProductsAsync();
        var dtos = products.Select(p => new ProductDto
        {
            Id = p.Id,
            Name = p.Name,
            Description = p.Description,
            Price = p.Price,
            ImageUrl = p.ImageUrl,
            CategoryId = p.CategoryId,
            CategoryName = p.CategoryName
        });
        return Ok(dtos);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var product = await _productService.GetProductByIdAsync(id);
        if (product == null) return NotFound();

        var dto = new ProductDto
        {
            Id = product.Id,
            Name = product.Name,
            Description = product.Description,
            Price = product.Price,
            ImageUrl = product.ImageUrl,
            CategoryId = product.CategoryId,
            CategoryName = product.CategoryName
        };
        return Ok(dto);
    }

    // Create product with image
    [HttpPost]
    public async Task<IActionResult> Create([FromForm] ProductCreateDto dto)
    {
        string? imageUrl = null;

        if (dto.Image != null)
        {
            var uploadsDir = Path.Combine(_env.WebRootPath, "images");
            if (!Directory.Exists(uploadsDir))
                Directory.CreateDirectory(uploadsDir);

            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(dto.Image.FileName);
            var filePath = Path.Combine(uploadsDir, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await dto.Image.CopyToAsync(stream);
            }

            imageUrl = $"/images/{fileName}";
        }

        var product = new Product
        {
            Name = dto.Name,
            Description = dto.Description,
            Price = dto.Price,
            ImageUrl = imageUrl,
            CategoryId = dto.CategoryId
        };

        await _productService.AddProductAsync(product);
        return CreatedAtAction(nameof(GetById), new { id = product.Id }, product);
    }

    // Update product with optional image
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromForm] ProductCreateDto dto)
    {
        var existing = await _productService.GetProductByIdAsync(id);
        if (existing == null) return NotFound();

        if (dto.Image != null)
        {
            var uploadsDir = Path.Combine(_env.WebRootPath, "images");
            if (!Directory.Exists(uploadsDir))
                Directory.CreateDirectory(uploadsDir);

            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(dto.Image.FileName);
            var filePath = Path.Combine(uploadsDir, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await dto.Image.CopyToAsync(stream);
            }

            existing.ImageUrl = $"/images/{fileName}";
        }

        existing.Name = dto.Name;
        existing.Description = dto.Description;
        existing.Price = dto.Price;
        existing.CategoryId = dto.CategoryId;

        await _productService.UpdateProductAsync(existing);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _productService.DeleteProductAsync(id);
        return NoContent();
    }
}
