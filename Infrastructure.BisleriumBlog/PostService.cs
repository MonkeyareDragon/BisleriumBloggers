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
    public class PostService : IPostService
    {
        private readonly ApplicationDBContext _dbContext;

        public PostService(ApplicationDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Post> AddPost(string userId, string title, string content, string imageUrl)
        {
            var post = new Post
            {
                PostId = Guid.NewGuid(),
                UserId = userId,
                Title = title,
                Content = content,
                ImageUrl = imageUrl,
                CreatedAt = DateTime.UtcNow
            };

            _dbContext.Posts.Add(post);
            await _dbContext.SaveChangesAsync();

            return post;
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

        public async Task<IEnumerable<Post>> GetAllPosts()
        {
            return await _dbContext.Posts.ToListAsync();
        }

        public async Task<IEnumerable<Post>> GetPostbyId(string id)
        {
            var result = await _dbContext.Posts.Where(s => s.PostId.ToString() == id).ToListAsync();
            return result;
        }

        public async Task<Post?> UpdatePost(Post post)
        {
            var selectedPost = await _dbContext.Posts.FindAsync(post.PostId);
            if (selectedPost != null)
            {
                selectedPost.Title = post.Title;
                selectedPost.Content = post.Content;
                selectedPost.ImageUrl = post.ImageUrl;
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
    }
}
