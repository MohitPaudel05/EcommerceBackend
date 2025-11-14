using Ecommerce.Datas;
using Ecommerce.Dtos;
using Ecommerce.Interfaces;
using Ecommerce.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ecommerce.Repositories
{
    public class ProductRepository : GenericRepository<Product>, IProductRepository
    {
        public ProductRepository(ApplicationDbContext context) : base(context) { }

        public async Task<IEnumerable<Product>> GetAllProductsWithCategoriesAsync()
        {
            return await _context.Products
                                 .Include(p => p.ProductCategories)
                                 .ThenInclude(pc => pc.Category)
                                 .ToListAsync();
        }

        public async Task<Product?> GetProductWithCategoriesByIdAsync(int id)
        {
            return await _context.Products
                                 .Include(p => p.ProductCategories)
                                 .ThenInclude(pc => pc.Category)
                                 .FirstOrDefaultAsync(p => p.Id == id);
        }
        public async Task<IEnumerable<ProductDto>> GetAllProductsFromSPAsync()
        {
            var products = await _context.Set<ProductQueryResult>()
                .FromSqlRaw("EXEC sp_GetAllProducts")
                .AsNoTracking()
                .ToListAsync();

            return products.Select(p => new ProductDto
            {
                Id = p.Id,
                Name = p.Name ?? string.Empty,
                Description = p.Description ?? string.Empty,
                Price = p.Price ?? 0m,
                ImageUrl = p.ImageUrl,
                CategoryNames = (p.CategoryNames ?? "")
                                .Split(',', System.StringSplitOptions.RemoveEmptyEntries)
                                .Select(c => c.Trim())
                                .ToList()
            });
        }
    }
}
