using Application.BisleriumBlog;
using Domain.BisleriumBlog.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Domain.BisleriumBlog.View_Model.DashboardModels;

namespace Infrastructure.BisleriumBlog
{
    public class DashboardService : IDashboardService
    {
        private readonly ApplicationDBContext _dbContext;

        public DashboardService(ApplicationDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<DashboardCounts> GetDashboardCounts()
        {
            var userCount = await _dbContext.Users.CountAsync();
            var postCount = await _dbContext.Posts.CountAsync();
            var totalCommentCount = await _dbContext.Comments.CountAsync() + await _dbContext.Replys.CountAsync();
            var upvoteCount = await _dbContext.Votes.CountAsync(v => v.VoteType == VoteType.Upvote);
            var downvoteCount = await _dbContext.Votes.CountAsync(v => v.VoteType == VoteType.Downvote);

            return new DashboardCounts
            {
                UserCount = userCount,
                PostCount = postCount,
                TotalCommentCount = totalCommentCount,
                UpvoteCount = upvoteCount,
                DownvoteCount = downvoteCount
            };
        }

        public async Task<DashboardCounts> GetDashboardCountsOnChoosenTime(DateTime startDate, DateTime endDate)
        {
            var userCount = await _dbContext.Users.Where(u => u.CreatedAt >= startDate && u.CreatedAt <= endDate).CountAsync();
            var postCount = await _dbContext.Posts.Where(p => p.CreatedAt >= startDate && p.CreatedAt <= endDate).CountAsync();
            var totalCommentCount = await _dbContext.Comments.Where(c => c.CreatedAt >= startDate && c.CreatedAt <= endDate).CountAsync() + await _dbContext.Replys.Where(r => r.CreatedAt >= startDate && r.CreatedAt <= endDate).CountAsync();
            var upvoteCount = await _dbContext.Votes.Where(v => v.CreatedAt >= startDate && v.CreatedAt <= endDate && v.VoteType == VoteType.Upvote).CountAsync();
            var downvoteCount = await _dbContext.Votes.Where(v => v.CreatedAt >= startDate && v.CreatedAt <= endDate && v.VoteType == VoteType.Downvote).CountAsync();

            return new DashboardCounts
            {
                UserCount = userCount,
                PostCount = postCount,
                TotalCommentCount = totalCommentCount,
                UpvoteCount = upvoteCount,
                DownvoteCount = downvoteCount
            };
        }
    }
}
