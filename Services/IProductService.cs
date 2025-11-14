using Ecommerce.Dtos;
using Microsoft.AspNetCore.Hosting;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Ecommerce.Interfaces
{
    public interface IProductService
    {
        Task<IEnumerable<ProductDto>> GetAllProductsAsync();
        Task<ProductDto?> GetProductByIdAsync(int id);
        Task<ProductDto> CreateProductAsync(ProductCreateDto dto, IWebHostEnvironment env);
        Task<bool> UpdateProductAsync(int id, ProductCreateDto dto, IWebHostEnvironment env);
        Task<bool> DeleteProductAsync(int id);

        //spmethod
        Task<IEnumerable<ProductDto>> GetAllProductsFromSPAsync();
    }
}
