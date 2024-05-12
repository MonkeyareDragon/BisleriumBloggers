using Domain.BisleriumBlog.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Domain.BisleriumBlog.View_Model.RequestModel;

namespace Application.BisleriumBlog
{
    public interface IVoteService
    {
        Task<bool> CreateVoteAsync(VoteRequestModel model);
        Task<bool> UpdateVoteAsync(string userId, Guid? postId, Guid? commentId, Guid? replyId, VoteType newVoteType);
        Task<bool> DeleteVoteAsync(string userId, Guid? postId, Guid? commentId, Guid? replyId);
    }
}
