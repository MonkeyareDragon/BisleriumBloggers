using Domain.BisleriumBlog.Model;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.BisleriumBlog
{
    public class ApplicationDBContext : IdentityDbContext<AppUser>
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=DESKTOP-ID9J0PP\\SQLEXPRESS;Database=BisleriumBloggers;Trusted_Connection=True;TrustServerCertificate=True;MultipleActiveResultSets=True");
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            //One to Many Relationship between User and Post
            builder.Entity<Post>()
                .HasOne(p => p.User)
                .WithMany(p => p.Posts)
                .HasForeignKey(p => p.UserId)
                .IsRequired()
                .OnDelete(DeleteBehavior.ClientCascade);

            // One-to-Many Relationship between Post and Comment
            builder.Entity<Comment>()
                .HasOne(c => c.Post)
                .WithMany(p => p.Comments)
                .HasForeignKey(c => c.PostId)
                .IsRequired()
                .OnDelete(DeleteBehavior.ClientCascade);

            // One-to-Many Relationship between User and Comment
            builder.Entity<Comment>()
                .HasOne(c => c.User)
                .WithMany(u => u.Comments)
                .HasForeignKey(c => c.UserId)
                .IsRequired()
                .OnDelete(DeleteBehavior.ClientCascade);

            //One-to-Many Relationship between Comment and Reply
            builder.Entity<Reply>()
                .HasOne(r => r.Comment)
                .WithMany(r => r.Replys)
                .HasForeignKey(r => r.CommentId)
                .IsRequired()
                .OnDelete(DeleteBehavior.ClientCascade);

            //One-to-Many Relationship between User and Relpy
            builder.Entity<Reply>()
                .HasOne(c => c.User)
                .WithMany(r => r.Replys)
                .HasForeignKey(c => c.UserId)
                .IsRequired()
                .OnDelete(DeleteBehavior.ClientCascade);

            // Many-to-One Relationship between User and Vote
            builder.Entity<Vote>()
                .HasOne(v => v.User)
                .WithMany(u => u.Votes)
                .HasForeignKey(v => v.UserId)
                .IsRequired()
                .OnDelete(DeleteBehavior.ClientCascade);

            // Many-to-One Relationship between Post and Vote
            builder.Entity<Vote>()
                .HasOne(v => v.Post)
                .WithMany(p => p.Votes)
                .HasForeignKey(v => v.PostId)
                .OnDelete(DeleteBehavior.ClientCascade);

            // Many-to-One Relationship between Comment and Vote
            builder.Entity<Vote>()
                .HasOne(v => v.Comment)
                .WithMany(c => c.Votes)
                .HasForeignKey(v => v.CommentId)
                .OnDelete(DeleteBehavior.ClientCascade);

            // Many-to-One Relationship between Reply and Vote
            builder.Entity<Vote>()
                .HasOne(v => v.Reply)
                .WithMany(r => r.Votes)
                .HasForeignKey(v => v.ReplyId)
                .OnDelete(DeleteBehavior.ClientCascade);

            // Many-to-One Relationship between Reply and Vote
            builder.Entity<Notification>()
                .HasOne(v => v.User)
                .WithMany(r => r.Notifications)
                .HasForeignKey(v => v.UserId)
                .OnDelete(DeleteBehavior.ClientCascade);

            // Enum for VoteType
            builder.Entity<Vote>()
                .Property(v => v.VoteType)
                .HasConversion<string>();

            // Ensure unique constraint for (UserId, PostId), (UserId, CommentId), (UserId, ReplyId)
            builder.Entity<Vote>()
                .HasIndex(v => new { v.UserId, v.PostId })
                .IsUnique();

            builder.Entity<Vote>()
                .HasIndex(v => new { v.UserId, v.CommentId })
                .IsUnique();

            builder.Entity<Vote>()
                .HasIndex(v => new { v.UserId, v.ReplyId })
                .IsUnique();
        }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Reply> Replys { get; set; }
        public DbSet<Vote> Votes { get; set; }
        public DbSet<Notification> Notifications { get; set; }
    }
}
