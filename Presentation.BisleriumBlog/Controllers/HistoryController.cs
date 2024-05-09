using Application.BisleriumBlog;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using static Domain.BisleriumBlog.View_Model.SendViewModels;

namespace Presentation.BisleriumBlog.Controllers
{
    public class HistoryController : Controller
    {
        private readonly IHistoryService _historyService;

        public HistoryController(IHistoryService historyService)
        {
            _historyService = historyService;
        }

        [HttpPost("history/add")]
        [Authorize(Roles = "Admin, Blogger")]
        public async Task<IActionResult> AddHistory(HistoryDTO model)
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (string.IsNullOrEmpty(userId))
                {
                    return BadRequest("User id claim not found in token.");
                }

                var history = await _historyService.AddHistory(userId, model.PostId, model.CommentId, model.PreviousContent, model.UpdatedContent);
                return Ok(history);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("history/get")]
        [Authorize(Roles = "Admin, Blogger")]
        public async Task<IActionResult> GetAllHistory()
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (string.IsNullOrEmpty(userId))
                {
                    return BadRequest("User id claim not found in token.");
                }

                var history = await _historyService.GetAllHistory(userId);
                return Ok(history);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

    }
}
