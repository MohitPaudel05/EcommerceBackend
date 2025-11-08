using Ecommerce.Interfaces;
using Ecommerce.Models;

namespace Ecommerce.Services
{
    public class ProductService : IProductService
    {
        private readonly IUnitOfWork _unitOfWork;
        public ProductService(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;

        public async Task<IEnumerable<Product>> GetAllProductsAsync() =>
            await _unitOfWork.Products.GetAllProductsAsync();

        public async Task<Product?> GetProductByIdAsync(int id) =>
            await _unitOfWork.Products.GetProductWithCategoryByIdAsync(id);

        public async Task<IEnumerable<Product>> GetProductsByCategoryAsync(int categoryId) =>
            await _unitOfWork.Products.GetProductsByCategoryAsync(categoryId);

        public async Task AddProductAsync(Product product)
        {
            await _unitOfWork.Products.AddAsync(product);
            await _unitOfWork.SaveAsync();
        }

        public async Task UpdateProductAsync(Product product)
        {
            _unitOfWork.Products.Update(product);
            await _unitOfWork.SaveAsync();
        }

        public async Task DeleteProductAsync(int id)
        {
            var product = await _unitOfWork.Products.GetByIdAsync(id);
            if (product != null)
            {
                _unitOfWork.Products.Delete(product);
                await _unitOfWork.SaveAsync();
            }
        }
    }
}
