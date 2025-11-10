using Ecommerce.Dtos;

namespace Ecommerce.Interfaces
{
    public interface ICategoryService
    {
        Task<IEnumerable<CategoryDto>> GetAllCategoriesAsync();
        Task<CategoryDto?> GetCategoryByIdAsync(int id);
        Task<CategoryDto> CreateCategoryAsync(CategoryCreateDto dto);
        Task<bool> UpdateCategoryAsync(int id, CategoryCreateDto dto);
        Task<bool> DeleteCategoryAsync(int id);
    }
}
