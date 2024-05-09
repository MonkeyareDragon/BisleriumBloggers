using Domain.BisleriumBlog.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.BisleriumBlog
{
    public interface INotificationService
    {
        Task<Notification> AddNotification(string userId, string notificationNote);
        Task<IEnumerable<Notification>> GetAllNotifications(string userID);
    }
}
