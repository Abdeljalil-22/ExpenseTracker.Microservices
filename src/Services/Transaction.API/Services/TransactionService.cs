using Microsoft.EntityFrameworkCore;
using Transaction.API.Data;
using Transaction.API.Models;

namespace Transaction.API.Services
{
    public class TransactionService : ITransactionService
    {
        private readonly TransactionDbContext _context;

        public TransactionService(TransactionDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<TransactionItem>> GetAllUserTransactionsAsync(string userId)
        {
            return await _context.Transactions
                .Where(t => t.UserId == userId)
                .OrderByDescending(t => t.Date)
                .ToListAsync();
        }

        public async Task<TransactionItem> GetTransactionByIdAsync(int id, string userId)
        {
            var transaction = await _context.Transactions
                .FirstOrDefaultAsync(t => t.Id == id && t.UserId == userId);

            if (transaction == null)
            {
                throw new KeyNotFoundException($"Transaction with id {id} not found");
            }

            return transaction;
        }

        public async Task<TransactionItem> CreateTransactionAsync(TransactionCreateModel model, string userId)
        {
            var transaction = new TransactionItem
            {
                Title = model.Title,
                Description = model.Description,
                Amount = model.Amount,
                Date = model.Date,
                CategoryId = model.CategoryId,
                TransactionType = model.TransactionType,
                UserId = userId
            };

            _context.Transactions.Add(transaction);
            await _context.SaveChangesAsync();

            return transaction;
        }

        public async Task<TransactionItem> UpdateTransactionAsync(int id, TransactionUpdateModel model, string userId)
        {
            var transaction = await _context.Transactions
                .FirstOrDefaultAsync(t => t.Id == id && t.UserId == userId);

            if (transaction == null)
            {
                throw new KeyNotFoundException($"Transaction with id {id} not found");
            }

            transaction.Title = model.Title;
            transaction.Description = model.Description;
            transaction.Amount = model.Amount;
            transaction.Date = model.Date;
            transaction.CategoryId = model.CategoryId;
            transaction.TransactionType = model.TransactionType;

            await _context.SaveChangesAsync();
            return transaction;
        }

        public async Task<bool> DeleteTransactionAsync(int id, string userId)
        {
            var transaction = await _context.Transactions
                .FirstOrDefaultAsync(t => t.Id == id && t.UserId == userId);

            if (transaction == null)
            {
                return false;
            }

            _context.Transactions.Remove(transaction);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<decimal> GetTotalIncomeAsync(string userId, DateTime? startDate = null, DateTime? endDate = null)
        {
            var query = _context.Transactions
                .Where(t => t.UserId == userId && t.TransactionType == 1); // Income = 1

            if (startDate.HasValue)
            {
                query = query.Where(t => t.Date >= startDate.Value);
            }

            if (endDate.HasValue)
            {
                query = query.Where(t => t.Date <= endDate.Value);
            }

            return await query.SumAsync(t => t.Amount);
        }

        public async Task<decimal> GetTotalExpensesAsync(string userId, DateTime? startDate = null, DateTime? endDate = null)
        {
            var query = _context.Transactions
                .Where(t => t.UserId == userId && t.TransactionType == 2); // Expense = 2

            if (startDate.HasValue)
            {
                query = query.Where(t => t.Date >= startDate.Value);
            }

            if (endDate.HasValue)
            {
                query = query.Where(t => t.Date <= endDate.Value);
            }

            return await query.SumAsync(t => t.Amount);
        }
    }
}