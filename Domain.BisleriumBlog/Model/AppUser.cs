using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.BisleriumBlog.Model
{
    public class AppUser : IdentityUser
    {
        public string? Address { get; set; }
        public string? Gender { get; set; }
        public DateTime? CreatedAt { get; set; }
        public ICollection<Post>? Posts { get; set; }
        public ICollection<Comment>? Comments { get; set; }
        public ICollection<Reply>? Replys { get; set; }
        public ICollection<Vote>? Votes { get; set; }
        public ICollection<Notification>? Notifications { get; set; }
        public ICollection<History>? Historys { get; set; }
    }
}
