using Application.BisleriumBlog;
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

        [HttpGet, Route("dashboard/counts")]
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
    }
}
