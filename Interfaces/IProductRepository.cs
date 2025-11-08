using Ecommerce.Models;

namespace Ecommerce.Interfaces
{
    public interface  IProductRepository: IGenericRepository<Product>
    {
     
        Task<IEnumerable<Product>> GetAllProductsAsync(); // Products with Category
        Task<Product?> GetProductWithCategoryByIdAsync(int id); // Single product with Category
        Task<IEnumerable<Product>> GetProductsByCategoryAsync(int categoryId); // Products filtered by Category
    };
    }

