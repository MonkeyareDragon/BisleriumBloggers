using Application.BisleriumBlog;
using Domain.BisleriumBlog;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.BisleriumBlog.Controllers
{
    public class PostController : Controller
    {
        public readonly IPostService _postService;

        public PostController(IPostService postService)
        {
            _postService = postService;
        }

        [HttpPost, Route("AddPost")]
        public async Task<IActionResult> AddPost(Post post)
        {
            var addPost = await _postService.AddPost(post);
            return Ok(addPost);
        }

        [HttpGet, Route("GetAllPosts")]
        public async Task<IActionResult> GetAllPost()
        {
            return Ok(await _postService.GetAllPosts());
        }

        [HttpGet, Route("GetPostByID")]
        public async Task<IActionResult> GetPostByID(string id)
        {
            var result = await _postService.GetPostbyId(id);
            return Ok(result);
        }

        [HttpDelete, Route("DeletePostByID")]
        public async Task<IActionResult> DeletePostByID(string id)
        {
            await _postService.DeletePost(id);
            return Ok();
        }

        [HttpPut, Route("UpdatePostByID")]
        public async Task<IActionResult> UpdateStudentByID(Post post)
        {
            var updatedPost = await _postService.UpdatePost(post);
            if (updatedPost != null)
            {
                return Ok(updatedPost);
            }
            else
            {
                return NotFound(new { Error = "Post not found." });
            }
        }
    }
}
