using Ecommerce.Dtos;
using Ecommerce.Interfaces;
using Ecommerce.Models;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Ecommerce.Services
{
    public class ProductService : IProductService
    {
        private readonly IUnitOfWork _unitOfWork;
        public ProductService(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;

        public async Task<IEnumerable<ProductDto>> GetAllProductsAsync()
        {
            var products = await _unitOfWork.Products.GetAllProductsWithCategoriesAsync();
            return products.Select(p => new ProductDto
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                Price = p.Price,
                ImageUrl = p.ImageUrl,
                CategoryNames = p.ProductCategories.Select(pc => pc.Category.Name).ToList()
            });
        }

        public async Task<ProductDto?> GetProductByIdAsync(int id)
        {
            var product = await _unitOfWork.Products.GetProductWithCategoriesByIdAsync(id);
            if (product == null) return null;

            return new ProductDto
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                ImageUrl = product.ImageUrl,
                CategoryNames = product.ProductCategories.Select(pc => pc.Category.Name).ToList()
            };
        }

        public async Task<ProductDto> CreateProductAsync(ProductCreateDto dto, IWebHostEnvironment env)
        {
            string? imageUrl = await SaveImageAsync(dto.Image, env);

            var product = new Product
            {
                Name = dto.Name,
                Description = dto.Description,
                Price = dto.Price,
                ImageUrl = imageUrl
            };

            // Link multiple categories
            foreach (var catId in dto.CategoryIds)
            {
                product.ProductCategories.Add(new ProductCategory { CategoryId = catId });
            }

            await _unitOfWork.Products.AddAsync(product);
            await _unitOfWork.SaveAsync();

            return await GetProductByIdAsync(product.Id) ?? throw new Exception("Error creating product");
        }

        public async Task<bool> UpdateProductAsync(int id, ProductCreateDto dto, IWebHostEnvironment env)
        {
            var existing = await _unitOfWork.Products.GetProductWithCategoriesByIdAsync(id);
            if (existing == null) return false;

            if (dto.Image != null)
            {
                existing.ImageUrl = await SaveImageAsync(dto.Image, env);
            }

            existing.Name = dto.Name;
            existing.Description = dto.Description;
            existing.Price = dto.Price;

            // Update categories
            existing.ProductCategories.Clear();
            foreach (var catId in dto.CategoryIds)
            {
                existing.ProductCategories.Add(new ProductCategory { CategoryId = catId });
            }

            _unitOfWork.Products.Update(existing);
            await _unitOfWork.SaveAsync();
            return true;
        }

        public async Task<bool> DeleteProductAsync(int id)
        {
            var product = await _unitOfWork.Products.GetByIdAsync(id);
            if (product == null) return false;

            _unitOfWork.Products.Delete(product);
            await _unitOfWork.SaveAsync();
            return true;
        }

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
