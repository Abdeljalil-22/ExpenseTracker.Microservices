using Category.API.Models;
using Microsoft.EntityFrameworkCore;

namespace Category.API.Data
{
    public class CategoryDbContext : DbContext
    {
        public CategoryDbContext(DbContextOptions<CategoryDbContext> options) : base(options)
        {
        }

        public DbSet<CategoryItem> Categories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Add unique constraint for category name per user
            modelBuilder.Entity<CategoryItem>()
                .HasIndex(c => new { c.Name, c.UserId, c.Type })
                .IsUnique();
        }
    }
}