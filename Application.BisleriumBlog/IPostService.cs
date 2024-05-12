using Domain.BisleriumBlog.Model;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Domain.BisleriumBlog.View_Model.SendViewModels;

namespace Application.BisleriumBlog
{
    public interface IPostService
    {
        Task<Post?> AddPost(string userId, string title, string content, string imageUrl, IFormFile imageFile);
        Task<List<PostDTO>> GetAllPosts();
        Task<IEnumerable<PostDTO>> GetAllPostsSorted();
        Task<IEnumerable<PostDTO>> GetAllPostsSortedByPopularity();
        Task<IEnumerable<PostDTO>> GetRandomPosts();
        Task<Post?> UpdatePost(Post post, IFormFile imageFile);
        Task DeletePost(string id);
        Task<IEnumerable<Post>> GetPostbyId(string id);
        Task<PostDTO?> GetPostByIdResponse(string id);
        Task<IEnumerable<object>> GetCommentsWithReplies(Guid postId);
    }
}
