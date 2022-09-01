using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UserCollectionBlaz.Migrations
{
    public partial class stage19 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_AspNetUsers_AutorId",
                table: "Comments");

            migrationBuilder.RenameColumn(
                name: "AutorId",
                table: "Comments",
                newName: "AppUser");

            migrationBuilder.RenameIndex(
                name: "IX_Comments_AutorId",
                table: "Comments",
                newName: "IX_Comments_AppUser");

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_AspNetUsers_AppUser",
                table: "Comments",
                column: "AppUser",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_AspNetUsers_AppUser",
                table: "Comments");

            migrationBuilder.RenameColumn(
                name: "AppUser",
                table: "Comments",
                newName: "AutorId");

            migrationBuilder.RenameIndex(
                name: "IX_Comments_AppUser",
                table: "Comments",
                newName: "IX_Comments_AutorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_AspNetUsers_AutorId",
                table: "Comments",
                column: "AutorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
