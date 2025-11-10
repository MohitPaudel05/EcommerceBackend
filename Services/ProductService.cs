using Ecommerce.Dtos;
using Ecommerce.Interfaces;
using Ecommerce.Models;

namespace Ecommerce.Services
{
    public class ProductService : IProductService
    {
        private readonly IUnitOfWork _unitOfWork;
        public ProductService(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;

        // Get all
        public async Task<IEnumerable<ProductDto>> GetAllProductsAsync()
        {
            var products = await _unitOfWork.Products.GetAllProductsAsync();
            return products.Select(p => new ProductDto
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                Price = p.Price,
                ImageUrl = p.ImageUrl,
                CategoryId = p.CategoryId,
                CategoryName = p.CategoryName
            });
        }

        // Get by ID
        public async Task<ProductDto?> GetProductByIdAsync(int id)
        {
            var product = await _unitOfWork.Products.GetProductWithCategoryByIdAsync(id);
            if (product == null) return null;

            return new ProductDto
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                ImageUrl = product.ImageUrl,
                CategoryId = product.CategoryId,
                CategoryName = product.CategoryName
            };
        }

        // Create with image
        public async Task<ProductDto> CreateProductAsync(ProductCreateDto dto, IWebHostEnvironment env)
        {
            string? imageUrl = await SaveImageAsync(dto.Image, env);

            var product = new Product
            {
                Name = dto.Name,
                Description = dto.Description,
                Price = dto.Price,
                ImageUrl = imageUrl,
                CategoryId = dto.CategoryId
            };

            await _unitOfWork.Products.AddAsync(product);
            await _unitOfWork.SaveAsync();

            return new ProductDto
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                ImageUrl = product.ImageUrl,
                CategoryId = product.CategoryId
            };
        }

        // Update with optional image
        public async Task<bool> UpdateProductAsync(int id, ProductCreateDto dto, IWebHostEnvironment env)
        {
            var existing = await _unitOfWork.Products.GetByIdAsync(id);
            if (existing == null) return false;

            if (dto.Image != null)
            {
                existing.ImageUrl = await SaveImageAsync(dto.Image, env);
            }

            existing.Name = dto.Name;
            existing.Description = dto.Description;
            existing.Price = dto.Price;
            existing.CategoryId = dto.CategoryId;

            _unitOfWork.Products.Update(existing);
            await _unitOfWork.SaveAsync();
            return true;
        }

        // Delete
        public async Task<bool> DeleteProductAsync(int id)
        {
            var product = await _unitOfWork.Products.GetByIdAsync(id);
            if (product == null) return false;

            _unitOfWork.Products.Delete(product);
            await _unitOfWork.SaveAsync();
            return true;
        }

        // 🔹 Helper method for image saving
        private async Task<string?> SaveImageAsync(IFormFile? image, IWebHostEnvironment env)
        {
            if (image == null) return null;

            var uploadsDir = Path.Combine(env.WebRootPath, "images");
            if (!Directory.Exists(uploadsDir))
                Directory.CreateDirectory(uploadsDir);

            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(image.FileName);
            var filePath = Path.Combine(uploadsDir, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await image.CopyToAsync(stream);
            }

            return $"/images/{fileName}";
        }
    }
}
