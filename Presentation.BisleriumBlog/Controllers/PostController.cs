using Application.BisleriumBlog;
using Domain.BisleriumBlog.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using static Domain.BisleriumBlog.View_Model.RequestModel;

namespace Presentation.BisleriumBlog.Controllers
{
    public class PostController : Controller
    {
        public readonly IPostService _postService;

        public PostController(IPostService postService)
        {
            _postService = postService;
        }

        [HttpPost, Route("post/add")]
        [Authorize(Roles = "Admin, Blogger")]
        public async Task<IActionResult> AddPost([FromBody] PostRequestModel model)
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (string.IsNullOrEmpty(userId))
                {
                    return BadRequest("User id claim not found in token.");
                }

                var post = await _postService.AddPost(userId, model.Title, model.Content, model.ImageUrl);
                return Ok(post);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet, Route("post/get-all")]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllPost()
        {
            return Ok(await _postService.GetAllPosts());
        }

        [HttpGet, Route("post/get-by-id")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetPostByID(string id)
        {
            var result = await _postService.GetPostbyId(id);
            return Ok(result);
        }

        [HttpDelete, Route("post/delete")]
        [Authorize(Roles = "Admin, Blogger")]
        public async Task<IActionResult> DeletePostByID(string id)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var posts = await _postService.GetPostbyId(id);
            var post = posts.FirstOrDefault(); // Get the first post in case of multiple results

            if (post == null)
            {
                return NotFound();
            }

            if (User.IsInRole("Admin") || post.UserId == userId)
            {
                await _postService.DeletePost(id);
                return Ok();
            }

            return Forbid("Sorry, you are not authorized to delete this resource.");
        }

        [HttpPut, Route("post/update")]
        [Authorize(Roles = "Admin, Blogger")]
        public async Task<IActionResult> UpdatePostByID(Guid postId, [FromBody] PostRequestModel model)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            // Retrieve the post by ID from the database
            var existingPost = await _postService.GetPostbyId(postId.ToString());

            if (existingPost == null || !existingPost.Any())
            {
                return NotFound();
            }

            // Get the first post from the list
            var post = existingPost.First(); 

            // Check if the user is authorized to update the post
            if (post.UserId == userId || User.IsInRole("Admin"))
            {
                post.Title = model.Title;
                post.Content = model.Content;
                post.ImageUrl = model.ImageUrl;
                post.UpdatedAt = DateTime.UtcNow;

                // Update the post in the database
                var updatedPost = await _postService.UpdatePost(post);

                if (updatedPost != null)
                {
                    return Ok(updatedPost);
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
