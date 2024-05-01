using Application.BisleriumBlog;
using Domain.BisleriumBlog.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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

        private int CalculatePopularity(Post post)
        {
            // Calculate the upvotes and downvotes for the given post
            int upvotes = _dbContext.Votes.Count(v => v.PostId == post.PostId && v.VoteType == VoteType.Upvote);
            int downvotes = _dbContext.Votes.Count(v => v.PostId == post.PostId && v.VoteType == VoteType.Downvote);
            int commentsCount = post.Comments != null ? post.Comments.Count : 0;

            // Calculate and return the popularity score for the given post
            int popularity = (upvotes * 2) + (downvotes * -1) + commentsCount;
            return popularity;
        }

        public async Task<List<Post>> GetMostPopularPostsAllTime()
        {
            var posts = await _dbContext.Posts
                .Include(p => p.Comments)
                .ToListAsync();

            // Order posts by popularity using LINQ without modifying the Post class
            var popularPosts = posts.OrderByDescending(p => CalculatePopularity(p)).Take(10).ToList();

            return popularPosts;
        }

        public async Task<List<Post>> GetMostPopularPostsChosenMonth(int month)
        {
            var posts = await _dbContext.Posts
                .Include(p => p.Comments)
                .ToListAsync();

            // Filter posts based on the chosen month
            posts = posts.Where(p => p.CreatedAt?.Month == month).ToList();

            // Order posts by popularity using LINQ without modifying the Post class
            var popularPosts = posts.OrderByDescending(p => CalculatePopularity(p)).Take(10).ToList();

            return popularPosts;
        }

        public async Task<List<AppUser>> GetMostPopularBloggersAllTime()
        {
            var posts = await _dbContext.Posts
                .Include(p => p.User)
                .ToListAsync();

            var bloggers = posts.GroupBy(p => p.UserId)
                .Select(g => new
                {
                    UserId = g.Key,
                    PopularityScore = g.Sum(p => CalculatePopularity(p))
                })
                .OrderByDescending(g => g.PopularityScore)
                .Take(10)
                .ToList();

            var popularBloggers = await _dbContext.Users
                .Where(u => bloggers.Select(b => b.UserId).Contains(u.Id))
                .ToListAsync();

            return popularBloggers;
        }

        public async Task<List<AppUser>> GetMostPopularBloggersChosenMonth(int month)
        {
            var startDate = new DateTime(DateTime.Now.Year, month, 1);
            var endDate = startDate.AddMonths(1).Date;

            var posts = await _dbContext.Posts
                .Include(p => p.User)
                .Where(p => p.CreatedAt >= startDate && p.CreatedAt <= endDate)
                .ToListAsync();

            var bloggers = posts.GroupBy(p => p.UserId)
                .Select(g => new
                {
                    UserId = g.Key,
                    PopularityScore = g.Sum(p => CalculatePopularity(p))
                })
                .OrderByDescending(g => g.PopularityScore)
                .Take(10)
                .ToList();

            var popularBloggers = await _dbContext.Users
                .Where(u => bloggers.Select(b => b.UserId).Contains(u.Id))
                .ToListAsync();

            return popularBloggers;
        }
    }
}
