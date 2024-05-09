using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.BisleriumBlog.Migrations
{
    /// <inheritdoc />
    public partial class PhotoModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ImageUrl",
                table: "Posts",
                newName: "ProfileImage");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ProfileImage",
                table: "Posts",
                newName: "ImageUrl");
        }
    }
}
