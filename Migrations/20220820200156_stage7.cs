using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UserCollectionBlaz.Migrations
{
    public partial class stage7 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Collection_AspNetUsers_OwnerId",
                table: "Collection");

            migrationBuilder.DropForeignKey(
                name: "FK_Item_Collection_collectionId",
                table: "Item");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Collection",
                table: "Collection");

            migrationBuilder.RenameTable(
                name: "Collection",
                newName: "Collections");

            migrationBuilder.RenameIndex(
                name: "IX_Collection_OwnerId",
                table: "Collections",
                newName: "IX_Collections_OwnerId");

            migrationBuilder.AddColumn<string>(
                name: "ItemType",
                table: "Collections",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Collections",
                table: "Collections",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Collections_AspNetUsers_OwnerId",
                table: "Collections",
                column: "OwnerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Item_Collections_collectionId",
                table: "Item",
                column: "collectionId",
                principalTable: "Collections",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Collections_AspNetUsers_OwnerId",
                table: "Collections");

            migrationBuilder.DropForeignKey(
                name: "FK_Item_Collections_collectionId",
                table: "Item");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Collections",
                table: "Collections");

            migrationBuilder.DropColumn(
                name: "ItemType",
                table: "Collections");

            migrationBuilder.RenameTable(
                name: "Collections",
                newName: "Collection");

            migrationBuilder.RenameIndex(
                name: "IX_Collections_OwnerId",
                table: "Collection",
                newName: "IX_Collection_OwnerId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Collection",
                table: "Collection",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Collection_AspNetUsers_OwnerId",
                table: "Collection",
                column: "OwnerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Item_Collection_collectionId",
                table: "Item",
                column: "collectionId",
                principalTable: "Collection",
                principalColumn: "Id");
        }
    }
}
