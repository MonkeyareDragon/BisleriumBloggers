using Application.BisleriumBlog;
using Domain.BisleriumBlog.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Domain.BisleriumBlog.View_Model.SendViewModels;

namespace Infrastructure.BisleriumBlog
{
    public class NotificationService : INotificationService
    {
        private readonly ApplicationDBContext _dbContext;

        public NotificationService(ApplicationDBContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<Notification> AddNotification(NotificationSummaryDTO notificationDTO)
        {
            var post = await _dbContext.Posts
                .Include(p => p.User)
                .FirstOrDefaultAsync(p => p.PostId == notificationDTO.PostId);

            if (post == null)
            {
                throw new ArgumentException("Post not found");
            }

            var notification = new Notification
            {
                UserId = post.UserId,
                PostId = notificationDTO.PostId,
                Note = notificationDTO.NotificationNote,
                CreatedAt = DateTime.UtcNow
            };

            _dbContext.Notifications.Add(notification);
            await _dbContext.SaveChangesAsync();

            return notification;
        }

        public async Task<IEnumerable<NotificationDTO>> GetAllNotifications(string userId)
        {
            var notifications = await _dbContext.Notifications
                .Include(n => n.Post)
                .Where(n => n.UserId == userId)
                .OrderByDescending(n => n.CreatedAt)
                .ToListAsync();

            // Map notifications to DTOs with associated post image URL
            var notificationDTOs = notifications.Select(n => new NotificationDTO
            {
                NotificationId = n.NotificationID,
                UserId = n.UserId,
                PostId = n.PostId,
                Note = n.Note,
                CreatedAt = n.CreatedAt,
                PostImage = n.Post != null ? n.Post.ProfileImage : null 
            }).ToList();

            return notificationDTOs;
        }

        public async Task DeleteNotification(Guid notificationId)
        {
            var notification = await _dbContext.Notifications.FindAsync(notificationId);
            if (notification != null)
            {
                _dbContext.Notifications.Remove(notification);
                await _dbContext.SaveChangesAsync();
            }
        }
    }
}
