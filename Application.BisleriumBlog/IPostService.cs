using Domain.BisleriumBlog.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.BisleriumBlog
{
    public interface IPostService
    {
        Task<Post> AddPost(string userId, string title, string content, string imageUrl);
        Task<IEnumerable<Post>> GetAllPosts();
        Task<Post?> UpdatePost(Post post);
        Task DeletePost(string id);
        Task<IEnumerable<Post>> GetPostbyId(string id);
    }
}
