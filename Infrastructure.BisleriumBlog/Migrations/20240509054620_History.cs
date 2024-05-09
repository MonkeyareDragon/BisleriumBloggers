using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.BisleriumBlog.Migrations
{
    /// <inheritdoc />
    public partial class History : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_History_AspNetUsers_UserId",
                table: "History");

            migrationBuilder.DropForeignKey(
                name: "FK_History_Comments_CommentId",
                table: "History");

            migrationBuilder.DropForeignKey(
                name: "FK_History_Posts_PostId",
                table: "History");

            migrationBuilder.DropForeignKey(
                name: "FK_Notification_AspNetUsers_UserId",
                table: "Notification");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Notification",
                table: "Notification");

            migrationBuilder.DropPrimaryKey(
                name: "PK_History",
                table: "History");

            migrationBuilder.RenameTable(
                name: "Notification",
                newName: "Notifications");

            migrationBuilder.RenameTable(
                name: "History",
                newName: "Historys");

            migrationBuilder.RenameIndex(
                name: "IX_Notification_UserId",
                table: "Notifications",
                newName: "IX_Notifications_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_History_UserId",
                table: "Historys",
                newName: "IX_Historys_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_History_PostId",
                table: "Historys",
                newName: "IX_Historys_PostId");

            migrationBuilder.RenameIndex(
                name: "IX_History_CommentId",
                table: "Historys",
                newName: "IX_Historys_CommentId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Notifications",
                table: "Notifications",
                column: "NotificationID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Historys",
                table: "Historys",
                column: "HistoryID");

            migrationBuilder.AddForeignKey(
                name: "FK_Historys_AspNetUsers_UserId",
                table: "Historys",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Historys_Comments_CommentId",
                table: "Historys",
                column: "CommentId",
                principalTable: "Comments",
                principalColumn: "CommentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Historys_Posts_PostId",
                table: "Historys",
                column: "PostId",
                principalTable: "Posts",
                principalColumn: "PostId");

            migrationBuilder.AddForeignKey(
                name: "FK_Notifications_AspNetUsers_UserId",
                table: "Notifications",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Historys_AspNetUsers_UserId",
                table: "Historys");

            migrationBuilder.DropForeignKey(
                name: "FK_Historys_Comments_CommentId",
                table: "Historys");

            migrationBuilder.DropForeignKey(
                name: "FK_Historys_Posts_PostId",
                table: "Historys");

            migrationBuilder.DropForeignKey(
                name: "FK_Notifications_AspNetUsers_UserId",
                table: "Notifications");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Notifications",
                table: "Notifications");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Historys",
                table: "Historys");

            migrationBuilder.RenameTable(
                name: "Notifications",
                newName: "Notification");

            migrationBuilder.RenameTable(
                name: "Historys",
                newName: "History");

            migrationBuilder.RenameIndex(
                name: "IX_Notifications_UserId",
                table: "Notification",
                newName: "IX_Notification_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Historys_UserId",
                table: "History",
                newName: "IX_History_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Historys_PostId",
                table: "History",
                newName: "IX_History_PostId");

            migrationBuilder.RenameIndex(
                name: "IX_Historys_CommentId",
                table: "History",
                newName: "IX_History_CommentId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Notification",
                table: "Notification",
                column: "NotificationID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_History",
                table: "History",
                column: "HistoryID");

            migrationBuilder.AddForeignKey(
                name: "FK_History_AspNetUsers_UserId",
                table: "History",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_History_Comments_CommentId",
                table: "History",
                column: "CommentId",
                principalTable: "Comments",
                principalColumn: "CommentId");

            migrationBuilder.AddForeignKey(
                name: "FK_History_Posts_PostId",
                table: "History",
                column: "PostId",
                principalTable: "Posts",
                principalColumn: "PostId");

            migrationBuilder.AddForeignKey(
                name: "FK_Notification_AspNetUsers_UserId",
                table: "Notification",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
