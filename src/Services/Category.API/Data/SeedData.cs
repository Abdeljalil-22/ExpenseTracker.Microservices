using Category.API.Models;

namespace Category.API.Data
{
    public static class SeedData
    {
        public static async Task InitializeAsync(CategoryDbContext context)
        {
            // Skip if DB has been seeded
            if (context.Categories.Any())
            {
                return;
            }

            // System categories (UserId = null means system category)
            var expenseCategories = new List<CategoryItem>
            {
                new CategoryItem { Name = "Food & Dining", Type = 2, Icon = "utensils", IsSystem = true },
                new CategoryItem { Name = "Transportation", Type = 2, Icon = "car", IsSystem = true },
                new CategoryItem { Name = "Housing", Type = 2, Icon = "home", IsSystem = true },
                new CategoryItem { Name = "Utilities", Type = 2, Icon = "bolt", IsSystem = true },
               new CategoryItem { Name = "Entertainment", Type = 2, Icon = "film", IsSystem = true },
                new CategoryItem { Name = "Shopping", Type = 2, Icon = "shopping-bag", IsSystem = true },
                new CategoryItem { Name = "Health & Medical", Type = 2, Icon = "medkit", IsSystem = true },
                new CategoryItem { Name = "Personal Care", Type = 2, Icon = "user", IsSystem = true },
                new CategoryItem { Name = "Education", Type = 2, Icon = "graduation-cap", IsSystem = true },
                new CategoryItem { Name = "Other Expenses", Type = 2, Icon = "ellipsis-h", IsSystem = true }
            };

            var incomeCategories = new List<CategoryItem>
            {
                new CategoryItem { Name = "Salary", Type = 1, Icon = "money-check", IsSystem = true },
                new CategoryItem { Name = "Bonus", Type = 1, Icon = "gift", IsSystem = true },
                new CategoryItem { Name = "Investment", Type = 1, Icon = "chart-line", IsSystem = true },
                new CategoryItem { Name = "Freelance", Type = 1, Icon = "laptop", IsSystem = true },
                new CategoryItem { Name = "Other Income", Type = 1, Icon = "ellipsis-h", IsSystem = true }
            };

            await context.Categories.AddRangeAsync(expenseCategories);
            await context.Categories.AddRangeAsync(incomeCategories);
            await context.SaveChangesAsync();
        }
    }
}