using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UserCollectionBlaz.Migrations
{
    public partial class stage11 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "LikesPosition",
                table: "Item",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LikesPosition",
                table: "Comments",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Item_LikesPosition",
                table: "Item",
                column: "LikesPosition");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_LikesPosition",
                table: "Comments",
                column: "LikesPosition");

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_Likes_LikesPosition",
                table: "Comments",
                column: "LikesPosition",
                principalTable: "Likes",
                principalColumn: "Position");

            migrationBuilder.AddForeignKey(
                name: "FK_Item_Likes_LikesPosition",
                table: "Item",
                column: "LikesPosition",
                principalTable: "Likes",
                principalColumn: "Position");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Likes_LikesPosition",
                table: "Comments");

            migrationBuilder.DropForeignKey(
                name: "FK_Item_Likes_LikesPosition",
                table: "Item");

            migrationBuilder.DropIndex(
                name: "IX_Item_LikesPosition",
                table: "Item");

            migrationBuilder.DropIndex(
                name: "IX_Comments_LikesPosition",
                table: "Comments");

            migrationBuilder.DropColumn(
                name: "LikesPosition",
                table: "Item");

            migrationBuilder.DropColumn(
                name: "LikesPosition",
                table: "Comments");
        }
    }
}
