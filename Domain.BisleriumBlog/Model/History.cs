using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.BisleriumBlog.Model
{
    public class History
    {
        [Key]
        public Guid HistoryID { get; set; }
        [Required]
        public string? UserId { get; set; }
        public Guid? PostId { get; set; }
        public Guid? CommentId { get; set; }
        public DateTime? CreatedAt { get; set; }
        public string? previousContent { get; set; }
        public string? UpdatedContent { get; set; }
        public AppUser? User { get; set; }
        public Post? Post { get; set; }
        public Comment? Comment { get; set; }
    }
}
