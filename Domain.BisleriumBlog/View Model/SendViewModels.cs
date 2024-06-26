﻿using Domain.BisleriumBlog.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.BisleriumBlog.View_Model
{
    public class SendViewModels
    {
        public class PostDTO
        {
            public Guid Id { get; set; }
            public string? Author { get; set; }
            public string? CreateDate { get; set; }
            public string? UpdateDate { get; set; }
            public string? Image { get; set; }
            public int VoteCount { get; set; }
            public int CommentCount { get; set; }
            public string? Title { get; set; }
            public string? Description { get; set; }

            public Post First()
            {
                throw new NotImplementedException();
            }
        }

        public class PostSummaryDTO
        {
            public Guid PostId { get; set; }
            public string? Title { get; set; }
            public DateTime CreatedAt { get; set; }
            public int Popularity { get; set; }
            public int CommentsCount { get; set; }
        }

        public class UserPopularityDto
        {
            public string? UserId { get; set; }
            public string? Username { get; set; }
            public DateTime? CreatedAt { get; set; }
            public int PopularityScore { get; set; }
            public int TotalPosts { get; set; }
        }

        public class NotificationSummaryDTO
        {
            public Guid? PostId { get; set; }
            [Required]
            public string? NotificationNote { get; set; }
        }

        public class HistoryDTO
        {
            public Guid? PostId { get; set; }
            public Guid? CommentId { get; set; }
            [Required]
            public string? PreviousContent { get; set; }
            [Required]
            public string? UpdatedContent { get; set; }
        }

        public class NotificationDTO
        {
            public Guid? NotificationId { get; set; }
            public string? UserId { get; set; }
            public Guid? PostId { get; set; }
            public string? Note { get; set; }
            public DateTime? CreatedAt { get; set; }
            public string? PostImage { get; set; }
        }
    }
}
