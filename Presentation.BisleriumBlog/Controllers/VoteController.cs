using Application.BisleriumBlog;
using Domain.BisleriumBlog.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using static Domain.BisleriumBlog.View_Model.RequestModel;

namespace Presentation.BisleriumBlog.Controllers
{
    public class VoteController : Controller
    {
        private readonly IVoteService _voteService;
        public VoteController(IVoteService voteService)
        {
            _voteService = voteService;
        }

        [HttpPost, Route("vote/create")]
        [Authorize(Roles = "Admin, Blogger")]
        public async Task<IActionResult> CreateVote([FromBody] VoteRequestModel model)
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (string.IsNullOrEmpty(userId))
                {
                    return BadRequest("User id claim not found in token.");
                }

                var vote = await _voteService.CreateVote(userId, model.PostId, model.CommentId, model.ReplyId, model.VoteType);
                return Ok(vote);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpDelete, Route("vote/remove")]
        [AllowAnonymous]
        public async Task<IActionResult> RemoveVote(Guid voteId)
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (string.IsNullOrEmpty(userId))
                {
                    return BadRequest("User id claim not found in token.");
                }

                await _voteService.RemoveVote(userId, voteId);
                return Ok("Vote removed successfully.");
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPut, Route("vote/update")]
        [Authorize(Roles = "Admin, Blogger")]
        public async Task<IActionResult> UpdateVoteType(Guid voteId, VoteType newVoteType)
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (string.IsNullOrEmpty(userId))
                {
                    return BadRequest("User id claim not found in token.");
                }

                await _voteService.UpdateVoteType(userId, voteId, newVoteType);
                return Ok("Vote type updated successfully.");
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
