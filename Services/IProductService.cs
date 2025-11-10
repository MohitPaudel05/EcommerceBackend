using Ecommerce.Dtos;


namespace Ecommerce.Interfaces
{
    public interface IProductService
    {
        Task<IEnumerable<ProductDto>> GetAllProductsAsync();
        Task<ProductDto?> GetProductByIdAsync(int id);
        Task<ProductDto> CreateProductAsync(ProductCreateDto dto, IWebHostEnvironment env);
        Task<bool> UpdateProductAsync(int id, ProductCreateDto dto, IWebHostEnvironment env);
        Task<bool> DeleteProductAsync(int id);
    }
}