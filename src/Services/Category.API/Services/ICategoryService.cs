using Category.API.Models;

namespace Category.API.Services
{
    public interface ICategoryService
    {
        Task<IEnumerable<CategoryItem>> GetAllCategoriesAsync(string userId, int? type = null);
        Task<CategoryItem> GetCategoryByIdAsync(int id, string userId);
        Task<CategoryItem> CreateCategoryAsync(CategoryCreateModel model, string userId);
        Task<CategoryItem> UpdateCategoryAsync(int id, CategoryUpdateModel model, string userId);
        Task<bool> DeleteCategoryAsync(int id, string userId);
    }
}