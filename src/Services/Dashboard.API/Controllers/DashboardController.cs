using Dashboard.API.Models;
using Dashboard.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Dashboard.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class DashboardController : ControllerBase
    {
        private readonly IDashboardService _dashboardService;

        public DashboardController(IDashboardService dashboardService)
        {
            _dashboardService = dashboardService;
        }

        [HttpGet]
        public async Task<IActionResult> GetDashboard([FromQuery] DateTime? startDate, [FromQuery] DateTime? endDate)
        {
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }

            var dashboard = await _dashboardService.GetDashboardDataAsync(userId, startDate, endDate);
            return Ok(dashboard);
        }

        [HttpGet("categories/{type}")]
        public async Task<IActionResult> GetCategoryBreakdown(int type, [FromQuery] DateTime? startDate, [FromQuery] DateTime? endDate)
        {
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }

            var categories = await _dashboardService.GetCategoryBreakdownAsync(userId, type, startDate, endDate);
            return Ok(categories);
        }

        [HttpGet("trends")]
        public async Task<IActionResult> GetMonthlyTrends([FromQuery] int months = 6)
        {
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }

            var trends = await _dashboardService.GetMonthlyTrendsAsync(userId, months);
            return Ok(trends);
        }
    }
}