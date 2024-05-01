using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.BisleriumBlog.Model
{
    public class Reply
    {
        [Key]
        public Guid ReplyId { get; set; }
        [Required]
        public string? UserId { get; set; }
        [Required]
        public Guid CommentId { get; set; }
        [Required]
        public string? ReplyText { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public AppUser? User { get; set; }
        public Comment? Comment { get; set; }
    }
}
