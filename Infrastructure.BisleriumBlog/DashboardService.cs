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
using static Domain.BisleriumBlog.View_Model.SendViewModels;
using static System.Net.Mime.MediaTypeNames;

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

        public async Task<List<PostSummaryDTO>> GetMostPopularPostsAllTime()
        {
            var posts = await _dbContext.Posts
                .Include(p => p.Comments)
                .ToListAsync();

            // Project required details into instances of PostSummaryDTO
            var popularPosts = posts.OrderByDescending(p => CalculatePopularity(p))
                                    .Take(10)
                                    .Select(p => new PostSummaryDTO
                                    {
                                        PostId = p.PostId,
                                        Title = p.Title,
                                        CreatedAt = p.CreatedAt ?? DateTime.MinValue,
                                        Popularity = CalculatePopularity(p),
                                        CommentsCount = p.Comments != null ? p.Comments.Count : 0
                                    })
                                    .ToList();

            return popularPosts;
        }

        public async Task<List<PostSummaryDTO>> GetMostPopularPostsChosenMonth(int month)
        {
            var posts = await _dbContext.Posts
                .Include(p => p.Comments)
                .Where(p => p.CreatedAt != null && p.CreatedAt.Value.Month == month)
                .ToListAsync();

            // Project required details into instances of PostSummaryDTO
            var popularPosts = posts.OrderByDescending(p => CalculatePopularity(p))
                                    .Take(10)
                                    .Select(p => new PostSummaryDTO
                                    {
                                        PostId = p.PostId,
                                        Title = p.Title,
                                        CreatedAt = p.CreatedAt ?? DateTime.MinValue,
                                        Popularity = CalculatePopularity(p),
                                        CommentsCount = p.Comments != null ? p.Comments.Count : 0
                                    })
                                    .ToList();

            return popularPosts;
        }

        private int CalculatePopularityScore(AppUser user)
        {
            int upvoteWeightage = 2;
            int downvoteWeightage = -1;
            int commentWeightage = 1;

            int upvotes = _dbContext.Votes.Count(v => v.UserId == user.Id && v.VoteType == VoteType.Upvote);
            int downvotes = _dbContext.Votes.Count(v => v.UserId == user.Id && v.VoteType == VoteType.Downvote);
            int comments = _dbContext.Comments.Count(c => c.UserId == user.Id);

            return upvoteWeightage * upvotes + downvoteWeightage * downvotes + commentWeightage * comments;
        }

        public List<UserPopularityDto> GetMostPopularBloggersAllTime()
        {
            // Fetch users from the database into memory
            var users = _dbContext.Users.Include(u => u.Posts)
                                         .ThenInclude(p => p.Comments)
                                         .ToList();

            // Perform sorting and projection locally
            var userPopularity = users.Select(u => new UserPopularityDto
            {
                UserId = u.Id,
                Username = u.UserName,
                CreatedAt = u.CreatedAt,
                PopularityScore = CalculatePopularityScore(u),
                TotalPosts = u.Posts.Count
            })
                                    .OrderByDescending(u => u.PopularityScore)
                                    .Take(10)
                                    .ToList();

            return userPopularity;
        }

        public List<UserPopularityDto> GetMostPopularBloggersChosenMonth(int month)
        {
            var minPopularityScore = 0; // Set your minimum popularity score here
            var minTotalPosts = 0; // Set your minimum total posts here

            var users = _dbContext.Users
                .Select(u => new
                {
                    User = u,
                    TotalUpvotes = u.Posts.SelectMany(p => p.Votes).Count(v => v.VoteType == VoteType.Upvote),
                    TotalDownvotes = u.Posts.SelectMany(p => p.Votes).Count(v => v.VoteType == VoteType.Downvote),
                    TotalComments = u.Posts.SelectMany(p => p.Comments).Count()
                })
                .Where(u => u.User.CreatedAt != null && u.User.CreatedAt.Value.Month == month)
                .OrderByDescending(u => 2 * u.TotalUpvotes + (-1) * u.TotalDownvotes + u.TotalComments)
                .Take(10)
                .Select(u => new UserPopularityDto
                {
                    UserId = u.User.Id,
                    Username = u.User.UserName,
                    CreatedAt = u.User.CreatedAt,
                    TotalPosts = u.User.Posts.Count,
                    PopularityScore = 2 * u.TotalUpvotes + (-1) * u.TotalDownvotes + u.TotalComments
                })
                .Where(u => u.CreatedAt != null && u.CreatedAt.Value.Month == month &&
                            u.PopularityScore >= minPopularityScore &&
                            u.TotalPosts >= minTotalPosts)
                .ToList();

            return users;
        }
    }
}
