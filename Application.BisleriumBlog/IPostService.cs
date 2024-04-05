using Domain.BisleriumBlog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.BisleriumBlog
{
    public interface IPostService
    {
        Task<Post> AddPost(Post post);
        Task<IEnumerable<Post>> GetAllPost();
        Task<Post> UpdatePost(Post post);
        Task DeletePost(string id);
        Task<IEnumerable<Post>> GetPostbyId(string id);
    }
}
