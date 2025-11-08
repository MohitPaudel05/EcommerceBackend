using Ecommerce.Datas;
using Ecommerce.Interfaces;
using Ecommerce.Models;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Repositories
{
    public class ProductRepository : GenericRepository<Product>, IProductRepository
    {
        public ProductRepository(ApplicationDbContext context) : base(context) { }
        //load products with their categories
        public async Task<IEnumerable<Product>> GetAllProductsAsync()
        {
            return await _context.Products
                                 .Include(p => p.Category)
                                 .ToListAsync();
        }
        // load single category products by category id
        
        public async Task<Product?> GetProductWithCategoryByIdAsync(int id)
        {
            return await _context.Products
                                 .Include(p => p.Category)
                                 .FirstOrDefaultAsync(p => p.Id == id);
        }

        // Load products by category Id with Category
        public async Task<IEnumerable<Product>> GetProductsByCategoryAsync(int categoryId)
        {
            return await _context.Products
                                 .Where(p => p.CategoryId == categoryId)
                                 .Include(p => p.Category)
                                 .ToListAsync();
        }
    }
}