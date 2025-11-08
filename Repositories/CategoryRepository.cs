using Ecommerce.Datas;
using Ecommerce.Interfaces;
using Ecommerce.Models;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Repositories
{
    public class CategoryRepository : GenericRepository<Category>, ICategoryRepository
    {
        public CategoryRepository(ApplicationDbContext context) : base(context) { }
        public async Task<IEnumerable<Category>> GetAllCategoriesWithProductsAsync()
        {
            return await _context.Categories
                                 .Include(c => c.Products)
                                 .ToListAsync();
        }

        
    }
}
