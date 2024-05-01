using Application.BisleriumBlog;
using Infrastructure.BisleriumBlog;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using static Domain.BisleriumBlog.View_Model.RequestModel;

namespace Presentation.BisleriumBlog.Controllers
{
    public class CommentController : ControllerBase
    {
        private readonly ICommentService _commentService;

        public CommentController(ICommentService commentService)
        {
            _commentService = commentService;
        }

        [HttpPost, Route("comment/add")]
        [Authorize(Roles = "Admin, Blogger")]
        public async Task<IActionResult> AddComment([FromBody] CommentRequestModel model)
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (string.IsNullOrEmpty(userId))
                {
                    return BadRequest("User id claim not found in token.");
                }

                var comment = await _commentService.AddComment(userId, model.PostId, model.CommentText);
                return Ok(comment);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet, Route("comment/get-all")]
        [Authorize(Roles = "Admin, Blogger")]
        public async Task<IActionResult> GetAllComments()
        {
            try
            {
                var comments = await _commentService.GetAllComments();
                return Ok(comments);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet, Route("comment/get-by-id")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetCommentByID(string id)
        {
            var result = await _commentService.GetCommentbyId(id);
            return Ok(result);
        }

        [HttpDelete, Route("comment/delete")]
        [Authorize(Roles = "Admin, Blogger")]
        public async Task<IActionResult> DeletePostByID(string id)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var posts = await _commentService.GetCommentbyId(id);
            var post = posts.FirstOrDefault();

            if (post == null)
            {
                return NotFound();
            }

            if (User.IsInRole("Admin") || post.UserId == userId)
            {
                await _commentService.DeleteComment(id);
                return Ok();
            }

            return Forbid("Sorry, you are not authorized to delete this resource.");
        }

        [HttpPut, Route("comment/update")]
        [Authorize(Roles = "Admin, Blogger")]
        public async Task<IActionResult> UpdateCommentByID([FromBody] CommentRequestModel model)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            // Retrieve the comment by ID from the database
            var existingComment = await _commentService.GetCommentbyId(model.CommentId.ToString());

            if (existingComment == null || !existingComment.Any())
            {
                return NotFound();
            }

            // Get the first comment from the list
            var comment = existingComment.First();

            // Check if the user is authorized to update the comment
            if (comment.UserId == userId || User.IsInRole("Admin"))
            {
                comment.CommentText = model.CommentText;
                comment.UpdatedAt = DateTime.UtcNow;

                // Update the comment in the database
                var updatedComment = await _commentService.UpdateComment(comment);

                if (updatedComment != null)
                {
                    return Ok(updatedComment);
                }
                else
                {
                    return StatusCode(500, "Failed to update post.");
                }
            }
            // Return Forbidden if user is not authorized to update the post
            return Forbid();
        }
    }
}
