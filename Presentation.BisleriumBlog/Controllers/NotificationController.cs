﻿using Application.BisleriumBlog;
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

        [HttpPost("notification/add")]
        [Authorize(Roles = "Admin, Blogger")]
        public async Task<IActionResult> AddNotification([FromBody] NotificationSummaryDTO model)
        {
            try
            {
                var notification = await _notificationService.AddNotification(model);
                return Ok(notification);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("notification/get")]
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

        [HttpDelete("notification/delete")]
        [Authorize(Roles = "Admin, Blogger")]
        public async Task<IActionResult> DeleteNotification(Guid id)
        {
            await _notificationService.DeleteNotification(id);
            return Ok();
        }
    }
}
