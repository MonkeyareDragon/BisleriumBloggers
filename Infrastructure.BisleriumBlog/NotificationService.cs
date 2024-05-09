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
    public class NotificationService : INotificationService
    {
        private readonly ApplicationDBContext _dbContext;

        public NotificationService(ApplicationDBContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<Notification> AddNotification(String userId, string notificationNote)
        {
            var notification = new Notification
            {
                NotificationID = Guid.NewGuid(),
                UserId = userId,
                CreatedAt = DateTime.Now,
                Note = notificationNote
            };

            _dbContext.Notifications.Add(notification);
            await _dbContext.SaveChangesAsync();

            return notification;
        }

        public async Task<IEnumerable<Notification>> GetAllNotifications(string userId)
        {
            return await _dbContext.Notifications
                .Where(n => n.UserId == userId)
                .ToListAsync();
        }
    }
}
