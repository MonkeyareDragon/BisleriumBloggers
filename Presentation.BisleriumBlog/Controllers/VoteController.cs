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
                var created = await _voteService.CreateVoteAsync(model);
                return Ok("Vote created successfully.");
            }
            catch (ArgumentException e)
            {
                return BadRequest(e.Message);
            }
            catch (InvalidOperationException e)
            {
                return Conflict(e.Message);
            }
            catch (Exception)
            {
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        [HttpDelete, Route("vote/remove")]
        [AllowAnonymous]
        public async Task<IActionResult> DeleteVote([FromBody] VoteRequestModel model)
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (string.IsNullOrEmpty(userId))
                {
                    return BadRequest("User id claim not found in token.");
                }

                var deleted = await _voteService.DeleteVoteAsync(userId, model.PostId, model.CommentId, model.ReplyId);
                return Ok("Vote deleted successfully.");
            }
            catch (KeyNotFoundException e)
            {
                return NotFound(e.Message);
            }
            catch (Exception)
            {
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        [HttpPut, Route("vote/update")]
        [Authorize(Roles = "Admin, Blogger")]
        public async Task<IActionResult> UpdateVote([FromBody] VoteRequestModel model)
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (string.IsNullOrEmpty(userId))
                {
                    return BadRequest("User id claim not found in token.");
                }

                var updated = await _voteService.UpdateVoteAsync(userId, model.PostId, model.CommentId, model.ReplyId, model.VoteType);
                return Ok("Vote updated successfully.");
            }
            catch (KeyNotFoundException e)
            {
                return NotFound(e.Message);
            }
            catch (Exception)
            {
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }
    }
}
