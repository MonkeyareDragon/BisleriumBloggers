using Application.BisleriumBlog;
using Domain.BisleriumBlog.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.BisleriumBlog
{
    public class VoteService : IVoteService
    {
        private readonly ApplicationDBContext _dbContext;
        public VoteService(ApplicationDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Vote> CreateVote(string userId, Guid? postId, Guid? commentId, Guid? replyId, VoteType voteType)
        {
            var vote = new Vote
            {
                VoteId = Guid.NewGuid(),
                UserId = userId,
                PostId = postId,
                CommentId = commentId,
                ReplyId = replyId,
                VoteType = voteType,
                CreatedAt = DateTime.UtcNow
            };

            try
            {
                _dbContext.Votes.Add(vote);
                await _dbContext.SaveChangesAsync();
                return vote;
            }
            catch (DbUpdateException ex)
            {
                // Handle unique constraint violation
                if (ex.InnerException is Microsoft.Data.SqlClient.SqlException sqlException && sqlException.Number == 2601)
                {
                    // Unique constraint violation
                    throw new InvalidOperationException("You have already voted on this post, comment, or reply.");
                }
                throw;
            }
        }

        public async Task RemoveVote(string userId, Guid voteId)
        {
            var vote = await _dbContext.Votes.FirstOrDefaultAsync(v => v.VoteId == voteId && v.UserId == userId);

            if (vote != null)
            {
                _dbContext.Votes.Remove(vote);
                await _dbContext.SaveChangesAsync();
            }
            else
            {
                throw new InvalidOperationException("Vote not found or you are not authorized to remove this vote.");
            }
        }

        public async Task UpdateVoteType(string userId, Guid voteId, VoteType newVoteType)
        {
            var vote = await _dbContext.Votes.FirstOrDefaultAsync(v => v.VoteId == voteId && v.UserId == userId);

            if (vote != null)
            {
                vote.VoteType = newVoteType;
                await _dbContext.SaveChangesAsync();
            }
            else
            {
                throw new InvalidOperationException("Vote not found or you are not authorized to update this vote.");
            }
        }
    }
}
