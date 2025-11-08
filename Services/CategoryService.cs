using Ecommerce.Interfaces;
using Ecommerce.Models;

namespace Ecommerce.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly IUnitOfWork _unitOfWork;
        public CategoryService(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;

        public async Task<IEnumerable<Category>> GetAllCategoriesAsync() =>
            await _unitOfWork.Categories.GetAllCategoriesWithProductsAsync();

        public async Task<Category?> GetCategoryByIdAsync(int id) =>
            await _unitOfWork.Categories.GetByIdAsync(id);

        public async Task AddCategoryAsync(Category category)
        {
            await _unitOfWork.Categories.AddAsync(category);
            await _unitOfWork.SaveAsync();
        }

        public async Task UpdateCategoryAsync(Category category)
        {
            _unitOfWork.Categories.Update(category);
            await _unitOfWork.SaveAsync();
        }

        public async Task DeleteCategoryAsync(int id)
        {
            var category = await _unitOfWork.Categories.GetByIdAsync(id);
            if (category != null)
            {
                _unitOfWork.Categories.Delete(category);
                await _unitOfWork.SaveAsync();
            }
        }
    }
}
