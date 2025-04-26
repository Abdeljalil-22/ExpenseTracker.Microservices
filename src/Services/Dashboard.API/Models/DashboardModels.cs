namespace Dashboard.API.Models
{
    public class FinancialSummary
    {
        public decimal TotalIncome { get; set; }
        public decimal TotalExpenses { get; set; }
        public decimal Balance { get; set; }
        public decimal SavingsRate { get; set; } // (Income - Expenses) / Income * 100
    }

    public class CategorySummary
    {
        public int CategoryId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Icon { get; set; }
        public decimal Amount { get; set; }
        public decimal Percentage { get; set; }
    }

    public class MonthlyData
    {
        public string Month { get; set; } = string.Empty;
        public decimal Income { get; set; }
        public decimal Expenses { get; set; }
        public decimal Balance { get; set; }
    }

    public class DashboardData
    {
        public FinancialSummary Summary { get; set; } = new FinancialSummary();
        public List<CategorySummary> TopExpenseCategories { get; set; } = new List<CategorySummary>();
        public List<CategorySummary> TopIncomeCategories { get; set; } = new List<CategorySummary>();
        public List<MonthlyData> MonthlyTrends { get; set; } = new List<MonthlyData>();
    }
}