using Category.API.Data;
using Category.API.Models;
using Microsoft.EntityFrameworkCore;

namespace Category.API.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly CategoryDbContext _context;

        public CategoryService(CategoryDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<CategoryItem>> GetAllCategoriesAsync(string userId, int? type = null)
        {
            // Get both system categories and user's custom categories
            var query = _context.Categories
                .Where(c => c.IsSystem || c.UserId == userId);

            if (type.HasValue)
            {
                query = query.Where(c => c.Type == type.Value);
            }

            return await query.OrderBy(c => c.Name).ToListAsync();
        }

        public async Task<CategoryItem> GetCategoryByIdAsync(int id, string userId)
        {
            var category = await _context.Categories
                .FirstOrDefaultAsync(c => c.Id == id && (c.IsSystem || c.UserId == userId));

            if (category == null)
            {
                throw new KeyNotFoundException($"Category with id {id} not found");
            }

            return category;
        }

        public async Task<CategoryItem> CreateCategoryAsync(CategoryCreateModel model, string userId)
        {
            // Check if a category with the same name already exists for this user and type
            var existingCategory = await _context.Categories
                .FirstOrDefaultAsync(c => c.Name == model.Name && c.UserId == userId && c.Type == model.Type);

            if (existingCategory != null)
            {
                throw new InvalidOperationException($"A category with name '{model.Name}' already exists");
            }

            var category = new CategoryItem
            {
                Name = model.Name,
                Description = model.Description,
                Type = model.Type,
                Icon = model.Icon,
                UserId = userId,
                IsSystem = false // User created categories are not system categories
            };

            _context.Categories.Add(category);
            await _context.SaveChangesAsync();

            return category;
        }

        public async Task<CategoryItem> UpdateCategoryAsync(int id, CategoryUpdateModel model, string userId)
        {
            var category = await _context.Categories
                .FirstOrDefaultAsync(c => c.Id == id && c.UserId == userId);

            if (category == null)
            {
                throw new KeyNotFoundException($"Category with id {id} not found");
            }

            if (category.IsSystem)
            {
                throw new InvalidOperationException("System categories cannot be modified");
            }

            // Check if updating to a name that already exists for this user and type
            var existingCategory = await _context.Categories
                .FirstOrDefaultAsync(c => c.Name == model.Name && c.UserId == userId && c.Type == category.Type && c.Id != id);

            if (existingCategory != null)
            {
                throw new InvalidOperationException($"A category with name '{model.Name}' already exists");
            }

            category.Name = model.Name;
            category.Description = model.Description;
            category.Icon = model.Icon;

            await _context.SaveChangesAsync();
            return category;
        }

        public async Task<bool> DeleteCategoryAsync(int id, string userId)
        {
            var category = await _context.Categories
                .FirstOrDefaultAsync(c => c.Id == id && c.UserId == userId);

            if (category == null)
            {
                return false;
            }

            if (category.IsSystem)
            {
                throw new InvalidOperationException("System categories cannot be deleted");
            }

            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}