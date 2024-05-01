using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.BisleriumBlog.Model
{
    public class Comment
    {
        [Key]
        public Guid CommentId { get; set; }
        [Required]
        public string? UserId { get; set; }
        [Required]
        public Guid PostId { get; set; }
        [Required]
        public string? CommentText { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public AppUser? User { get; set; }
        public Post? Post { get; set; }
        public ICollection<Reply>? Replys { get; set; }
        public ICollection<Vote>? Votes { get; set; }
    }
}
