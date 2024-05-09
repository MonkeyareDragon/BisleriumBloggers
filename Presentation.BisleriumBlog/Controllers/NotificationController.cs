using Application.BisleriumBlog;
using Infrastructure.BisleriumBlog;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static Domain.BisleriumBlog.View_Model.RequestModel;
using System.Security.Claims;
using static Domain.BisleriumBlog.View_Model.SendViewModels;

namespace Presentation.BisleriumBlog.Controllers
{
    public class NotificationController : Controller
    {
        private readonly INotificationService _notificationService;

        public NotificationController(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        [HttpPost("Notification/Add")]
        [Authorize(Roles = "Admin, Blogger")]
        public async Task<IActionResult> AddNotification([FromBody] NotificationSummaryDTO model)
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (string.IsNullOrEmpty(userId))
                {
                    return BadRequest("User id claim not found in token.");
                }

                var notification = await _notificationService.AddNotification(userId, model.NotificationNote);
                return Ok(notification);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("Notification/Get")]
        [Authorize(Roles = "Admin, Blogger")]
        public async Task<IActionResult> GetAllNotifications()
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (string.IsNullOrEmpty(userId))
                {
                    return BadRequest("User id claim not found in token.");
                }

                var notifications = await _notificationService.GetAllNotifications(userId);
                return Ok(notifications);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
