using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UserCollectionBlaz.Migrations
{
    public partial class stage16 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "LikesPosition",
                table: "Item",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Item_LikesPosition",
                table: "Item",
                column: "LikesPosition");

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
                name: "FK_Item_Likes_LikesPosition",
                table: "Item");

            migrationBuilder.DropIndex(
                name: "IX_Item_LikesPosition",
                table: "Item");

            migrationBuilder.DropColumn(
                name: "LikesPosition",
                table: "Item");
        }
    }
}
