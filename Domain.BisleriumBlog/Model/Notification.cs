using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.BisleriumBlog.Model
{
    public class Notification
    {
        [Key]
        public Guid NotificationID { get; set; }
        [Required]
        public string? UserId { get; set; }
        public DateTime? CreatedAt { get; set; }
        public string? Note { get; set; }
        public AppUser? User { get; set; }
    }
}