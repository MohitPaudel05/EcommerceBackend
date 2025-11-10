using Ecommerce.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Ecommerce.Interfaces
{
    public interface IProductRepository : IGenericRepository<Product>
    {
        Task<IEnumerable<Product>> GetAllProductsWithCategoriesAsync();
        Task<Product?> GetProductWithCategoriesByIdAsync(int id);
    }
}
