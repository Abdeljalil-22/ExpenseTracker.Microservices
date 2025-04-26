using Transaction.API.Models;

namespace Transaction.API.Services
{
    public interface ITransactionService
    {
        Task<IEnumerable<TransactionItem>> GetAllUserTransactionsAsync(string userId);
        Task<TransactionItem> GetTransactionByIdAsync(int id, string userId);
        Task<TransactionItem> CreateTransactionAsync(TransactionCreateModel model, string userId);
        Task<TransactionItem> UpdateTransactionAsync(int id, TransactionUpdateModel model, string userId);
        Task<bool> DeleteTransactionAsync(int id, string userId);
        Task<decimal> GetTotalIncomeAsync(string userId, DateTime? startDate = null, DateTime? endDate = null);
        Task<decimal> GetTotalExpensesAsync(string userId, DateTime? startDate = null, DateTime? endDate = null);
    }
}