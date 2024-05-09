using Application.BisleriumBlog;
using Application.BisleriumBlog.Utils;
using Domain.BisleriumBlog.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using static Domain.BisleriumBlog.View_Model.SendViewModels;
using static System.Net.Mime.MediaTypeNames;

namespace Infrastructure.BisleriumBlog
{
    public class PostService : IPostService
    {
        private readonly ApplicationDBContext _dbContext;
        private readonly IFileService _fileService;

        public PostService(ApplicationDBContext dbContext, IFileService fs)
        {
            _dbContext = dbContext;
            this._fileService = fs;
        }

        public async Task<Post?> AddPost(string userId, string title, string content, string imageUrl, IFormFile imageFile)
        {
            if (imageFile != null)
            {
                var fileResult = _fileService.SaveImage(imageFile);
                if (fileResult.Item1 == 1)
                {
                    var post = new Post
                    {
                        PostId = Guid.NewGuid(),
                        UserId = userId,
                        Title = title,
                        Content = content,
                        CreatedAt = DateTime.UtcNow,
                        ProfileImage = fileResult.Item2
                    };
                    _dbContext.Posts.Add(post);
                    await _dbContext.SaveChangesAsync();

                    return post;
                }
                return null;
            }
            else
            {
                return null;
            }

        }

        public async Task DeletePost(string id)
        {
            var post = await _dbContext.Posts.FindAsync(Guid.Parse(id));
            if (post != null)
            {
                _dbContext.Posts.Remove(post);
                await _dbContext.SaveChangesAsync();
            }
        }

        // Helper method to calculate vote count
        private int CalculateVoteCount(Post post)
        {
            var upvotes = post.Votes?.Count(v => v.VoteType == VoteType.Upvote) ?? 0;
            var downvotes = post.Votes?.Count(v => v.VoteType == VoteType.Downvote) ?? 0;
            return Math.Max(0, upvotes - downvotes);
        }

        // Helper method to calculate comment count
        private int CalculateCommentCount(Post post)
        {
            var commentCount = post.Comments?.Count ?? 0;
            var replyCount = post.Comments?.Sum(c => c.Replys?.Count ?? 0) ?? 0;
            return commentCount + replyCount;
        }

        public async Task<List<PostDTO>> GetAllPosts()
        {
            var posts = await _dbContext.Posts
                .Include(p => p.User)
                .Include(p => p.Comments)
                .ThenInclude(c => c.Replys)
                .Include(p => p.Votes) 
                .ToListAsync();

            var postDTOs = posts.Select(p => new PostDTO
            {
                Id = p.PostId,
                Author = p.User?.UserName,
                Image = p.ProfileImage,
                CreateDate = p.CreatedAt?.ToString("dd MMMM yyyy HH:mm:ss"),
                UpdateDate = p.UpdatedAt?.ToString("dd MMMM yyyy HH:mm:ss"),
                VoteCount = CalculateVoteCount(p),
                CommentCount = CalculateCommentCount(p),
                Title = p.Title,
                Description = p.Content
            }).ToList();

            return postDTOs;
        }

        public async Task<IEnumerable<PostDTO>> GetAllPostsSorted()
        {
            var posts = await _dbContext.Posts
                .Include(p => p.User)
                .Include(p => p.Comments)
                .OrderByDescending(p => p.CreatedAt)
                .ToListAsync();

            return posts.Select(post => new PostDTO
            {
                Id = post.PostId,
                Author = post.User?.UserName,
                CreateDate = post.CreatedAt?.ToString("dd MMMM yyyy HH:mm:ss"),
                UpdateDate = post.UpdatedAt?.ToString("dd MMMM yyyy HH:mm:ss"),
                Title = post.Title,
                Description = post.Content,
                Image = post.ProfileImage,
                VoteCount = post.Votes?.Count ?? 0,
                CommentCount = post.Comments?.Count ?? 0
            });
        }

        public async Task<IEnumerable<PostDTO>> GetAllPostsSortedByPopularity()
        {
            var posts = await _dbContext.Posts
                .Include(p => p.User)
                .Include(p => p.Votes)
                .Include(p => p.Comments)
                .ToListAsync();

            return posts.OrderByDescending(p => CalculatePopularity(p))
                        .Select(post => new PostDTO
                        {
                            Id = post.PostId,
                            Author = post.User?.UserName,
                            CreateDate = post.CreatedAt?.ToString("dd MMMM yyyy HH:mm:ss"),
                            UpdateDate = post.UpdatedAt?.ToString("dd MMMM yyyy HH:mm:ss"),
                            Title = post.Title,
                            Description = post.Content,
                            Image = post.ProfileImage,
                            VoteCount = post.Votes?.Count ?? 0,
                            CommentCount = post.Comments?.Count ?? 0
                        });
        }


        private double CalculatePopularity(Post post)
        {
            double upvotes = post.Votes?.Count(v => v.VoteType == VoteType.Upvote) ?? 0;
            double downvotes = post.Votes?.Count(v => v.VoteType == VoteType.Downvote) ?? 0;
            double comments = post.Comments?.Count ?? 0;

            // Custom weightage rates
            double upvoteWeightage = 2;
            double downvoteWeightage = -1;
            double commentWeightage = 1;

            return upvoteWeightage * upvotes + downvoteWeightage * downvotes + commentWeightage * comments;
        }

        public async Task<IEnumerable<PostDTO>> GetRandomPosts()
        {
            var posts = await _dbContext.Posts
                .Include(p => p.User)
                .Include(p => p.Votes)
                .Include(p => p.Comments)
                .ToListAsync();

            // Shuffle the list of posts randomly
            var shuffledPosts = posts.OrderBy(x => Guid.NewGuid()).ToList();

            return shuffledPosts.Select(post => new PostDTO
            {
                Id = post.PostId,
                Author = post.User?.UserName,
                CreateDate = post.CreatedAt?.ToString("dd MMMM yyyy HH:mm:ss"),
                UpdateDate = post.UpdatedAt?.ToString("dd MMMM yyyy HH:mm:ss"),
                Title = post.Title,
                Description = post.Content,
                Image = post.ProfileImage,
                VoteCount = post.Votes?.Count ?? 0,
                CommentCount = post.Comments?.Count ?? 0
            });
        }

        public async Task<IEnumerable<Post>> GetPostbyId(string id)
        {
            var result = await _dbContext.Posts.Where(s => s.PostId.ToString() == id).ToListAsync();
            return result;
        }

        public async Task<PostDTO?> GetPostByIdResponse(string id)
        {
            var post = await _dbContext.Posts
                .Include(p => p.User)
                .Include(p => p.Comments)
                .FirstOrDefaultAsync(p => p.PostId.ToString() == id);

            if (post == null)
            {
                // Handle case where post with the given ID doesn't exist
                return null;
            }

            return new PostDTO
            {
                Id = post.PostId,
                Author = post.User?.UserName,
                CreateDate = post.CreatedAt?.ToString("dd MMMM yyyy HH:mm:ss"),
                UpdateDate = post.UpdatedAt?.ToString("dd MMMM yyyy HH:mm:ss"),
                Title = post.Title,
                Description = post.Content,
                Image = post.ProfileImage,
                VoteCount = post.Votes?.Count ?? 0,
                CommentCount = post.Comments?.Count ?? 0
            };
        }


        public async Task<Post?> UpdatePost(Post post)
        {
            var selectedPost = await _dbContext.Posts.FindAsync(post.PostId);
            if (selectedPost != null)
            {
                selectedPost.Title = post.Title;
                selectedPost.Content = post.Content;
                selectedPost.UpdatedAt = DateTime.UtcNow;
                
                _dbContext.Entry(selectedPost).State = EntityState.Modified;
                await _dbContext.SaveChangesAsync();

                return selectedPost;
            }
            else
            {
                return null;
            }
        }

        public async Task<IEnumerable<object>> GetCommentsWithReplies(Guid postId)
        {
            var commentsWithReplies = await _dbContext.Comments
                .Include(c => c.Replys)
                .Where(c => c.PostId == postId)
                .Select(c => new
                {
                    commentid = c.CommentId,
                    author = c.User.UserName,
                    content = c.CommentText,
                    voteCount = (c.Votes.Count(v => v.VoteType == VoteType.Upvote)) - (c.Votes.Count(v => v.VoteType == VoteType.Downvote)),
                    createDate = c.CreatedAt,
                    replyCount = c.Replys.Count,
                    replies = c.Replys.Select(r => new
                    {
                        replyId = r.ReplyId,
                        author = r.User.UserName,
                        content = r.ReplyText,
                        voteCount = (r.Votes.Count(v => v.VoteType == VoteType.Upvote)) - (r.Votes.Count(v => v.VoteType == VoteType.Downvote)),
                        createDate = r.CreatedAt
                    })
                })
                .ToListAsync();

            return commentsWithReplies;
        }
    }
}
