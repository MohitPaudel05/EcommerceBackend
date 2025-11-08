using Ecommerce.Models;

namespace Ecommerce.Interfaces
{
    public interface ICategoryRepository : IGenericRepository<Category>
    {
        Task<IEnumerable<Category>> GetAllCategoriesWithProductsAsync(); // Categories with Products
       
    }
}
