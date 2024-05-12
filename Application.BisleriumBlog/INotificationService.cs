using Domain.BisleriumBlog.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Domain.BisleriumBlog.View_Model.SendViewModels;

namespace Application.BisleriumBlog
{
    public interface INotificationService
    {
        Task<Notification> AddNotification(NotificationSummaryDTO notificationDTO);
        Task<IEnumerable<NotificationDTO>> GetAllNotifications(string userID);
        Task DeleteNotification(Guid notificationId);
    }
}
