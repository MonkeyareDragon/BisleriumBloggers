using Application.BisleriumBlog;
using Domain.BisleriumBlog.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Domain.BisleriumBlog.View_Model.RequestModel;

namespace Infrastructure.BisleriumBlog
{
    public class VoteService : IVoteService
    {
        private readonly ApplicationDBContext _dbContext;
        public VoteService(ApplicationDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<bool> CreateVoteAsync(VoteRequestModel model)
        {
            // Ensure only one of PostId, CommentId, or ReplyId is provided
            if ((model.PostId.HasValue ? 1 : 0) + (model.CommentId.HasValue ? 1 : 0) + (model.ReplyId.HasValue ? 1 : 0) != 1)
            {
                throw new ArgumentException("Provide exactly one of PostId, CommentId, or ReplyId.");
            }

            // Check if the combination of UserId and provided Id is unique
            if (await _dbContext.Votes.AnyAsync(v =>
                v.UserId == model.UserId &&
                (v.PostId == model.PostId || v.CommentId == model.CommentId || v.ReplyId == model.ReplyId)))
            {
                throw new InvalidOperationException("Vote already exists for the given entity.");
            }

            var vote = new Vote
            {
                UserId = model.UserId,
                PostId = model.PostId,
                CommentId = model.CommentId,
                ReplyId = model.ReplyId,
                VoteType = model.VoteType,
                CreatedAt = DateTime.Now
            };

            _dbContext.Votes.Add(vote);
            await _dbContext.SaveChangesAsync();

            return true;
        }

        public async Task<bool> UpdateVoteAsync(string userId, Guid? postId, Guid? commentId, Guid? replyId, VoteType newVoteType)
        {
            var vote = await _dbContext.Votes.FirstOrDefaultAsync(v =>
                v.UserId == userId &&
                (v.PostId == postId || v.CommentId == commentId || v.ReplyId == replyId));

            if (vote == null)
            {
                throw new KeyNotFoundException("Vote not found.");
            }

            vote.VoteType = newVoteType;
            await _dbContext.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteVoteAsync(string userId, Guid? postId, Guid? commentId, Guid? replyId)
        {
            var vote = await _dbContext.Votes.FirstOrDefaultAsync(v =>
                v.UserId == userId &&
                (v.PostId == postId || v.CommentId == commentId || v.ReplyId == replyId));

            if (vote == null)
            {
                throw new KeyNotFoundException("Vote not found.");
            }

            _dbContext.Votes.Remove(vote);
            await _dbContext.SaveChangesAsync();

            return true;
        }
    }
}
