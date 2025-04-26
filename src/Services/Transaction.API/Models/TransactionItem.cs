using System.ComponentModel.DataAnnotations;

namespace Transaction.API.Models
{
    public class TransactionItem
    {
        public int Id { get; set; }

        [Required]
        public string Title { get; set; } = string.Empty;

        public string? Description { get; set; }

        [Required]
        public decimal Amount { get; set; }

        [Required]
        public DateTime Date { get; set; }

        public int CategoryId { get; set; }

        [Required]
        public string UserId { get; set; } = string.Empty;

        // TransactionType: Income = 1, Expense = 2
        [Required]
        public int TransactionType { get; set; }
    }

    public class TransactionCreateModel
    {
        [Required]
        public string Title { get; set; } = string.Empty;

        public string? Description { get; set; }

        [Required]
        public decimal Amount { get; set; }

        public DateTime Date { get; set; } = DateTime.Now;

        [Required]
        public int CategoryId { get; set; }

        [Required]
        public int TransactionType { get; set; }
    }

    public class TransactionUpdateModel
    {
        [Required]
        public string Title { get; set; } = string.Empty;

        public string? Description { get; set; }

        [Required]
        public decimal Amount { get; set; }

        [Required]
        public DateTime Date { get; set; }

        [Required]
        public int CategoryId { get; set; }

        [Required]
        public int TransactionType { get; set; }
    }
}