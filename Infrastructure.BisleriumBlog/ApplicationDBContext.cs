using Domain.BisleriumBlog;
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
        public DbSet<Post> Posts { get; set; }
    }
}
