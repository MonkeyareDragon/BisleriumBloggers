using Application.BisleriumBlog;
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
            var totalUsers = await _dbContext.Users.CountAsync();
            var totalPosts = await _dbContext.Posts.CountAsync();
            var totalComments = await _dbContext.Comments.CountAsync();
            var totalReplies = await _dbContext.Replys.CountAsync();
            var totalVotes = await _dbContext.Votes.CountAsync();

            // Count total comments including replies
            totalComments += totalReplies;

            return new DashboardCounts
            {
                TotalUsers = totalUsers,
                TotalPosts = totalPosts,
                TotalComments = totalComments,
                TotalVotes = totalVotes
            };
        }
    }
}
