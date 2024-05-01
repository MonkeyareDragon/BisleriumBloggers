using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.BisleriumBlog.Model
{
    public class Vote
    {
        [Key]
        public Guid VoteId { get; set; }

        [Required]
        public string? UserId { get; set; }

        public Guid? PostId { get; set; }

        public Guid? CommentId { get; set; }

        public Guid? ReplyId { get; set; }

        [Required]
        public VoteType VoteType { get; set; }

        public DateTime CreatedAt { get; set; }

        public virtual AppUser? User { get; set; }

        public virtual Post? Post { get; set; }

        public virtual Comment? Comment { get; set; }

        public virtual Reply? Reply { get; set; }
    }

    public enum VoteType
    {
        Upvote,
        Downvote
    }
}
