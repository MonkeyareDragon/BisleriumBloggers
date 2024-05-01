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
                .IsRequired();

            // One-to-Many Relationship between Post and Comment
            builder.Entity<Comment>()
                .HasOne(c => c.Post)
                .WithMany(p => p.Comments)
                .HasForeignKey(c => c.PostId)
                .IsRequired();

            // One-to-Many Relationship between User and Comment
            builder.Entity<Comment>()
                .HasOne(c => c.User)
                .WithMany(u => u.Comments)
                .HasForeignKey(c => c.UserId)
                .IsRequired();
        }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Comment> Comments { get; set; }
    }
}
