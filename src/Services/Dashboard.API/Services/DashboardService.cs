using Dashboard.API.Models;
using System;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;

namespace Dashboard.API.Services
{
    public class DashboardService : IDashboardService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<DashboardService> _logger;

        public DashboardService(
            IHttpClientFactory httpClientFactory,
            ILogger<DashboardService> logger)
        {
            _httpClientFactory = httpClientFactory;
            _logger = logger;
        }

        public async Task<DashboardData> GetDashboardDataAsync(string userId, DateTime? startDate = null, DateTime? endDate = null)
        {
            // If no dates provided, default to current month
            if (!startDate.HasValue)
            {
                startDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            }

            if (!endDate.HasValue)
            {
                endDate = DateTime.Now;
            }

            var summary = await GetFinancialSummaryAsync(userId, startDate.Value, endDate.Value);
            var topExpenses = await GetCategoryBreakdownAsync(userId, 2, startDate, endDate);
            var topIncome = await GetCategoryBreakdownAsync(userId, 1, startDate, endDate);
            var monthlyTrends = await GetMonthlyTrendsAsync(userId);

            return new DashboardData
            {
                Summary = summary,
                TopExpenseCategories = topExpenses.Take(5).ToList(), // Top 5 expense categories
                TopIncomeCategories = topIncome.Take(5).ToList(),    // Top 5 income categories
                MonthlyTrends = monthlyTrends
            };
        }

        private async Task<FinancialSummary> GetFinancialSummaryAsync(string userId, DateTime startDate, DateTime endDate)
        {
            var client = _httpClientFactory.CreateClient("TransactionService");

            // Prepare request with token
            var request = new HttpRequestMessage(HttpMethod.Get,
                $"api/transactions/summary?startDate={startDate:yyyy-MM-dd}&endDate={endDate:yyyy-MM-dd}");

            // Token will be handled by the HTTP client handler

            var response = await client.SendAsync(request);
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            var data = JsonSerializer.Deserialize<JsonElement>(content);

            decimal totalIncome = data.GetProperty("totalIncome").GetDecimal();
            decimal totalExpenses = data.GetProperty("totalExpenses").GetDecimal();
            decimal balance = data.GetProperty("balance").GetDecimal();

            decimal savingsRate = totalIncome > 0
                ? Math.Round((balance / totalIncome) * 100, 2)
                : 0;

            return new FinancialSummary
            {
                TotalIncome = totalIncome,
                TotalExpenses = totalExpenses,
                Balance = balance,
                SavingsRate = savingsRate
            };
        }

        public async Task<List<CategorySummary>> GetCategoryBreakdownAsync(string userId, int type, DateTime? startDate = null, DateTime? endDate = null)
        {
            // This would require aggregated data from both the Transaction and Category services
            // For this implementation, we'll create a simulated response based on dummy data
            // In a real implementation, we would fetch transaction data grouped by category

            // Implementation note: This would typically involve:
            // 1. Getting all transactions for the user in the date range
            // 2. Grouping them by category
            // 3. Joining with category data to get names and icons
            // 4. Calculating totals and percentages

            // Placeholder implementation
            var dummyData = new List<CategorySummary>();

            if (type == 1) // Income
            {
                dummyData = new List<CategorySummary>
                {
                    new CategorySummary { CategoryId = 1, Name = "Salary", Icon = "money-check", Amount = 5000, Percentage = 75 },
                    new CategorySummary { CategoryId = 2, Name = "Freelance", Icon = "laptop", Amount = 1000, Percentage = 15 },
                    new CategorySummary { CategoryId = 3, Name = "Investment", Icon = "chart-line", Amount = 500, Percentage = 7.5m },
                    new CategorySummary { CategoryId = 4, Name = "Other Income", Icon = "ellipsis-h", Amount = 200, Percentage = 2.5m }
                };
            }
            else // Expenses
            {
                dummyData = new List<CategorySummary>
                {
                    new CategorySummary { CategoryId = 5, Name = "Housing", Icon = "home", Amount = 1500, Percentage = 45 },
                    new CategorySummary { CategoryId = 6, Name = "Food & Dining", Icon = "utensils", Amount = 800, Percentage = 24 },
                    new CategorySummary { CategoryId = 7, Name = "Transportation", Icon = "car", Amount = 400, Percentage = 12 },
                    new CategorySummary { CategoryId = 8, Name = "Utilities", Icon = "bolt", Amount = 300, Percentage = 9 },
                    new CategorySummary { CategoryId = 9, Name = "Entertainment", Icon = "film", Amount = 200, Percentage = 6 },
                    new CategorySummary { CategoryId = 10, Name = "Other Expenses", Icon = "ellipsis-h", Amount = 100, Percentage = 4 }
                };
            }

            return dummyData;
        }

        public async Task<List<MonthlyData>> GetMonthlyTrendsAsync(string userId, int months = 6)
        {
            var result = new List<MonthlyData>();

            // Get the current date and go back specified number of months
            var currentDate = DateTime.Now;

            for (int i = months - 1; i >= 0; i--)
            {
                var date = currentDate.AddMonths(-i);
                var startOfMonth = new DateTime(date.Year, date.Month, 1);
                var endOfMonth = startOfMonth.AddMonths(1).AddDays(-1);

                // In a real implementation, we would get this data from the Transaction service
                // For now, use simulated data
                var monthName = startOfMonth.ToString("MMM");
                decimal income = 5000 - (i * 100) + (new Random().Next(-500, 500)); // Simulated fluctuation
                decimal expenses = 3000 + (i * 50) + (new Random().Next(-300, 300)); // Simulated fluctuation

                result.Add(new MonthlyData
                {
                    Month = monthName,
                    Income = income,
                    Expenses = expenses,
                    Balance = income - expenses
                });
            }

            return result;
        }
    }
}