using Ecommerce.Datas;
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
    }
}
