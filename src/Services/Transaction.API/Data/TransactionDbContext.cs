using Microsoft.EntityFrameworkCore;
using Transaction.API.Models;

namespace Transaction.API.Data
{
    public class TransactionDbContext : DbContext
    {
        public TransactionDbContext(DbContextOptions<TransactionDbContext> options) : base(options)
        {
        }

        public DbSet<TransactionItem> Transactions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure relationships and constraints
            modelBuilder.Entity<TransactionItem>()
                .Property(t => t.Amount)
                .HasColumnType("decimal(18,2)");
        }
    }
}
