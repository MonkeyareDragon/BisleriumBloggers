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
    public class HistoryService : IHistoryService
    {
        private readonly ApplicationDBContext _dbContext;
        public HistoryService(ApplicationDBContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<History> AddHistory(string userId, Guid? postId, Guid? commentId, string previousContent, string UpdatedContent)
        {
            var history = new History
            {
                HistoryID = Guid.NewGuid(),
                UserId = userId,
                PostId = postId,
                CommentId = commentId,
                CreatedAt = DateTime.Now,
                previousContent = previousContent,
                UpdatedContent = UpdatedContent
            };

            _dbContext.Historys.Add(history);
            await _dbContext.SaveChangesAsync();

            return history;
        }

        public async Task<IEnumerable<History>> GetAllHistory(string userId)
        {
            return await _dbContext.Historys
                .Where(h => h.UserId == userId)
                .OrderByDescending(h => h.CreatedAt)
                .ToListAsync();
        }

        public async Task<bool> DeleteHistoryAsync(Guid historyId)
        {
            var history = await _dbContext.Historys.FindAsync(historyId);
            if (history == null)
            {
                return false; // History record not found
            }

            _dbContext.Historys.Remove(history);
            await _dbContext.SaveChangesAsync();
            return true; // History record deleted successfully
        }
    }
}
