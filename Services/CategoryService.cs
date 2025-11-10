using Ecommerce.Dtos;
using Ecommerce.Interfaces;
using Ecommerce.Models;

namespace Ecommerce.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly IUnitOfWork _unitOfWork;
        public CategoryService(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;

        // Get all categories with their product names
        public async Task<IEnumerable<CategoryDto>> GetAllCategoriesAsync()
        {
            var categories = await _unitOfWork.Categories.GetAllCategoriesWithProductsAsync();
            return categories.Select(c => new CategoryDto
            {
                Id = c.Id,
                Name = c.Name,
                Description = c.Description,
                ProductNames = c.ProductCategories.Select(pc => pc.Product.Name).ToList()
            });
        }

        // Get single category by ID
        public async Task<CategoryDto?> GetCategoryByIdAsync(int id)
        {
            var category = await _unitOfWork.Categories.GetByIdAsync(id);
            if (category == null) return null;

            return new CategoryDto
            {
                Id = category.Id,
                Name = category.Name,
                Description = category.Description,
                ProductNames = category.ProductCategories.Select(pc => pc.Product.Name).ToList()
            };
        }

        // Create category
        public async Task<CategoryDto> CreateCategoryAsync(CategoryCreateDto dto)
        {
            var category = new Category
            {
                Name = dto.Name,
                Description = dto.Description
            };

            await _unitOfWork.Categories.AddAsync(category);
            await _unitOfWork.SaveAsync();

            return new CategoryDto
            {
                Id = category.Id,
                Name = category.Name,
                Description = category.Description,
                ProductNames = new List<string>()
            };
        }

        // Update category
        public async Task<bool> UpdateCategoryAsync(int id, CategoryCreateDto dto)
        {
            var existing = await _unitOfWork.Categories.GetByIdAsync(id);
            if (existing == null) return false;

            existing.Name = dto.Name;
            existing.Description = dto.Description;

            _unitOfWork.Categories.Update(existing);
            await _unitOfWork.SaveAsync();
            return true;
        }

        // Delete category
        public async Task<bool> DeleteCategoryAsync(int id)
        {
            var category = await _unitOfWork.Categories.GetByIdAsync(id);
            if (category == null) return false;

            _unitOfWork.Categories.Delete(category);
            await _unitOfWork.SaveAsync();
            return true;
        }
    }
}
