using Ecommerce.Datas;
using Ecommerce.Interfaces;
using Ecommerce.Models;

namespace Ecommerce.Repositories
{
    public class CategoryRepository : GenericRepository<Category>, ICategoryRepository
    {
        public CategoryRepository(ApplicationDbContext context) : base(context) { }
    }
}
