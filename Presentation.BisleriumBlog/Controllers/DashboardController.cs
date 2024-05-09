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

        [HttpGet, Route("dashboard/popular-posts-all-time")]
        [Authorize(Roles = "Admin")] 
        public async Task<IActionResult> GetPopularPostsAllTime()
        {
            try
            {
                var popularPosts = await _dashboardService.GetMostPopularPostsAllTime();
                return Ok(popularPosts);
            }
            catch(Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet, Route("dashboard/popular-posts-chosen-month")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetPopularPostsChosenMonth(int month)
        {
            try
            {
                var popularPosts = await _dashboardService.GetMostPopularPostsChosenMonth(month);
                return Ok(popularPosts);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet, Route("dashboard/popular-bloggers-all-time")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetPopularBloggers()
        {
            try
            {
                // Call the service method to get the 10 most popular bloggers
                var popularBloggers = await _dashboardService.GetMostPopularBloggersAllTime();
                return Ok(popularBloggers);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet, Route("dashboard/popular-bloggers-chosen-month")]
        public async Task<IActionResult> GetPopularBloggersChosenMonth(int month)
        {
            try
            {
                // Call the service method to get the most popular bloggers for the chosen month
                var popularBloggers = await _dashboardService.GetMostPopularBloggersChosenMonth(month);
                return Ok(popularBloggers);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
