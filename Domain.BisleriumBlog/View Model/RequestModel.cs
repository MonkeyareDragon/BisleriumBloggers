using Domain.BisleriumBlog.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.BisleriumBlog.View_Model
{
    public class RequestModel
    {
        public class PostRequestModel
        {
            public string? Title { get; set; }
            public string? Content { get; set; }
            public string? ImageUrl { get; set; }
        }
        public class CommentRequestModel
        {
            public Guid PostId { get; set; }
            public Guid CommentId { get; set; }
            public string? CommentText { get; set; }
        }
        public class ReplyRequestModel
        {
            public Guid CommentId { get; set; }
            public string? ReplyText { get; set; }
        }
        public class VoteRequestModel
        {
            public Guid? PostId { get; set; }
            public Guid? CommentId { get; set; }
            public Guid? ReplyId { get; set; }
            public VoteType VoteType { get; set; }
        }
    }
}
