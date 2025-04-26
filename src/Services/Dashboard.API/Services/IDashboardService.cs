using Dashboard.API.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Dashboard.API.Services
{
    public interface IDashboardService
    {
        Task<DashboardData> GetDashboardDataAsync(string userId, DateTime? startDate = null, DateTime? endDate = null);
        Task<List<CategorySummary>> GetCategoryBreakdownAsync(string userId, int type, DateTime? startDate = null, DateTime? endDate = null);
        Task<List<MonthlyData>> GetMonthlyTrendsAsync(string userId, int months = 6);
    }
}