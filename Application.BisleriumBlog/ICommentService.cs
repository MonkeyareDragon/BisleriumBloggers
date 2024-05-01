using Domain.BisleriumBlog.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.BisleriumBlog
{
    public interface ICommentService
    {
        Task<Comment> AddComment(string userId, Guid postId, string commentText);
        Task<IEnumerable<Comment>> GetAllComments();
        Task<IEnumerable<Comment>> GetCommentbyId(string id);
        Task DeleteComment(string id);
        Task<Comment> UpdateComment(Comment comment);
    }
}
