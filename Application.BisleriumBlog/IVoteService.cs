using Domain.BisleriumBlog.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.BisleriumBlog
{
    public interface IVoteService
    {
        Task<Vote> CreateVote(string userId, Guid? postId, Guid? commentId, Guid? replyId, VoteType voteType);
        Task RemoveVote(string userId, Guid voteId);
        Task UpdateVoteType(string userId, Guid voteId, VoteType newVoteType);
    }
}
