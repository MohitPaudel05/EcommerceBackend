using Ecommerce.Models;
using Ecommerce.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly IWebHostEnvironment _env;

        public ProductController(IProductService productService, IWebHostEnvironment env)
        {
            _productService = productService;
            _env = env;
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAll()
        {
            var products = await _productService.GetAllProductsAsync();
            return Ok(products);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var product = await _productService.GetProductByIdAsync(id);
            if (product == null) return NotFound($"Product with ID {id} not found.");
            return Ok(product);
        }

        [HttpGet("category/{categoryId}")]
        public async Task<IActionResult> GetByCategory(int categoryId)
        {
            var products = await _productService.GetProductsByCategoryAsync(categoryId);
            return Ok(products);
        }

        [HttpPost("add")]
        public async Task<IActionResult> Add([FromForm] Product product, IFormFile image)
        {
            if (image != null && image.Length > 0)
            {
                var imagesFolder = Path.Combine(_env.WebRootPath, "images");
                if (!Directory.Exists(imagesFolder))
                    Directory.CreateDirectory(imagesFolder);

                var fileName = Path.GetFileName(image.FileName);
                var filePath = Path.Combine(imagesFolder, fileName);

                using var stream = new FileStream(filePath, FileMode.Create);
                await image.CopyToAsync(stream);

                product.ImageUrl = $"/images/{fileName}";
            }

            await _productService.AddProductAsync(product);
            return CreatedAtAction(nameof(GetById), new { id = product.Id }, product);
        }

        [HttpPut("update/{id}")]
        public async Task<IActionResult> Update(int id, [FromForm] Product product, IFormFile? image)
        {
            var existing = await _productService.GetProductByIdAsync(id);
            if (existing == null) return NotFound($"Product with ID {id} not found.");

            existing.Name = product.Name;
            existing.Description = product.Description;
            existing.Price = product.Price;
            existing.CategoryId = product.CategoryId;

            if (image != null && image.Length > 0)
            {
                var imagesFolder = Path.Combine(_env.WebRootPath, "images");
                var fileName = Path.GetFileName(image.FileName);
                var filePath = Path.Combine(imagesFolder, fileName);

                using var stream = new FileStream(filePath, FileMode.Create);
                await image.CopyToAsync(stream);

                existing.ImageUrl = $"/images/{fileName}";
            }

            await _productService.UpdateProductAsync(existing);
            return Ok(existing);
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _productService.DeleteProductAsync(id);
            return Ok("Deleted successfully");
        }
    }
}
