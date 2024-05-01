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
    public class CommentService : ICommentService
    {
        private readonly ApplicationDBContext _dbContext;

        public CommentService(ApplicationDBContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<Comment> AddComment(string userId, Guid postId, string commentText)
        {
            var comment = new Comment
            {
                CommentId = Guid.NewGuid(),
                UserId = userId,
                PostId = postId,
                CommentText = commentText,
                CreatedAt = DateTime.UtcNow
            };

            _dbContext.Comments.Add(comment);
            await _dbContext.SaveChangesAsync();

            return comment;
        }
        public async Task<IEnumerable<Comment>> GetAllComments()
        {
            return await _dbContext.Comments.ToListAsync();
        }
        public async Task<IEnumerable<Comment>> GetCommentbyId(string id)
        {
            var result = await _dbContext.Comments.Where(s => s.CommentId.ToString() == id).ToListAsync();
            return result;
        }
        public async Task DeleteComment(string id)
        {
            var comment = await _dbContext.Comments.FindAsync(Guid.Parse(id));
            if (comment != null)
            {
                _dbContext.Comments.Remove(comment);
                await _dbContext.SaveChangesAsync();
            }
        }
        public async Task<Comment?> UpdateComment(Comment comment)
        {
            var selectedComment = await _dbContext.Comments.FindAsync(comment.CommentId);
            if (selectedComment != null)
            {
                selectedComment.CommentText = comment.CommentText;
                selectedComment.UpdatedAt = DateTime.UtcNow;

                _dbContext.Entry(selectedComment).State = EntityState.Modified;
                await _dbContext.SaveChangesAsync();

                return selectedComment;
            }
            else
            {
                return null;
            }
        }
    }
}
