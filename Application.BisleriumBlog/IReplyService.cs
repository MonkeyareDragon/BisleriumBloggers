using Domain.BisleriumBlog.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.BisleriumBlog
{
    public interface IReplyService
    {
        Task<Reply> AddReply(string userId, Guid commentId, string replyText);
        Task<IEnumerable<Reply>> GetAllReplies();
        Task<IEnumerable<Reply>> GetRepliesByCommentId(string commentId);
        Task DeleteReply(string replyId);
        Task<Reply?> UpdateReply(Reply reply);
    }
}
