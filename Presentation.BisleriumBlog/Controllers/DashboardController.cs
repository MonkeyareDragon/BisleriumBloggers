﻿using Application.BisleriumBlog;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.BisleriumBlog.Controllers
{
    public class DashboardController : Controller
    {
        private readonly IDashboardService _dashboardService;

        public DashboardController(IDashboardService dashboardService)
        {
            _dashboardService = dashboardService;
        }

        [HttpGet, Route("dashboard/all-time-counts")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetDashboardCounts()
        {
            try
            {
                var counts = await _dashboardService.GetDashboardCounts();
                return Ok(counts);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet, Route("dashboard/choosen-time-counts")]
        [Authorize(Roles = "Admin")] 
        public async Task<IActionResult> GetDashboardCountsOnChoosenTime(DateTime startDate, DateTime endDate)
        {
            try
            {
                var dashboardCounts = await _dashboardService.GetDashboardCountsOnChoosenTime(startDate, endDate);
                return Ok(dashboardCounts);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
