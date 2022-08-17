using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UserCollectionBlaz.Migrations
{
    public partial class stage3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AutorName",
                table: "comments");

            migrationBuilder.AddColumn<string>(
                name: "AutorId",
                table: "comments",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_comments_AutorId",
                table: "comments",
                column: "AutorId");

            migrationBuilder.AddForeignKey(
                name: "FK_comments_AspNetUsers_AutorId",
                table: "comments",
                column: "AutorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_comments_AspNetUsers_AutorId",
                table: "comments");

            migrationBuilder.DropIndex(
                name: "IX_comments_AutorId",
                table: "comments");

            migrationBuilder.DropColumn(
                name: "AutorId",
                table: "comments");

            migrationBuilder.AddColumn<string>(
                name: "AutorName",
                table: "comments",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
