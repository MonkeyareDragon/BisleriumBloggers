using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.BisleriumBlog.Model
{
    public class Post
    {
        [Key]
        public Guid PostId { get; set; }
        [Required]
        public string? UserId { get; set; }
        [Required]
        public string? Title { get; set; }
        public string? Content { get; set; }
        public string? ImageUrl { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public AppUser? User { get; set; }
        public ICollection<Comment>? Comments { get; set; }
        public ICollection<Vote>? Votes { get; set; }
    }
}
