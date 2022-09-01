using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UserCollectionBlaz.Migrations
{
    public partial class stage10 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_comments_AspNetUsers_AutorId",
                table: "comments");

            migrationBuilder.DropPrimaryKey(
                name: "PK_comments",
                table: "comments");

            migrationBuilder.RenameTable(
                name: "comments",
                newName: "Comments");

            migrationBuilder.RenameIndex(
                name: "IX_comments_AutorId",
                table: "Comments",
                newName: "IX_Comments_AutorId");

            migrationBuilder.AddColumn<string>(
                name: "LikePosition",
                table: "AspNetUsers",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Comments",
                table: "Comments",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "Likes",
                columns: table => new
                {
                    Position = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Likes", x => x.Position);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_LikePosition",
                table: "AspNetUsers",
                column: "LikePosition");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Likes_LikePosition",
                table: "AspNetUsers",
                column: "LikePosition",
                principalTable: "Likes",
                principalColumn: "Position");

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_AspNetUsers_AutorId",
                table: "Comments",
                column: "AutorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Likes_LikePosition",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_Comments_AspNetUsers_AutorId",
                table: "Comments");

            migrationBuilder.DropTable(
                name: "Likes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Comments",
                table: "Comments");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_LikePosition",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "LikePosition",
                table: "AspNetUsers");

            migrationBuilder.RenameTable(
                name: "Comments",
                newName: "comments");

            migrationBuilder.RenameIndex(
                name: "IX_Comments_AutorId",
                table: "comments",
                newName: "IX_comments_AutorId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_comments",
                table: "comments",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_comments_AspNetUsers_AutorId",
                table: "comments",
                column: "AutorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
