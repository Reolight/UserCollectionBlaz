using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UserCollectionBlaz.Migrations
{
    public partial class stage9 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tags_Item_ItemId",
                table: "Tags");

            migrationBuilder.DropIndex(
                name: "IX_Tags_ItemId",
                table: "Tags");

            migrationBuilder.DropColumn(
                name: "ItemId",
                table: "Tags");

            migrationBuilder.CreateTable(
                name: "ItemTag",
                columns: table => new
                {
                    ItemsId = table.Column<int>(type: "int", nullable: false),
                    TagsName = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItemTag", x => new { x.ItemsId, x.TagsName });
                    table.ForeignKey(
                        name: "FK_ItemTag_Item_ItemsId",
                        column: x => x.ItemsId,
                        principalTable: "Item",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ItemTag_Tags_TagsName",
                        column: x => x.TagsName,
                        principalTable: "Tags",
                        principalColumn: "Name",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ItemTag_TagsName",
                table: "ItemTag",
                column: "TagsName");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ItemTag");

            migrationBuilder.AddColumn<int>(
                name: "ItemId",
                table: "Tags",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Tags_ItemId",
                table: "Tags",
                column: "ItemId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tags_Item_ItemId",
                table: "Tags",
                column: "ItemId",
                principalTable: "Item",
                principalColumn: "Id");
        }
    }
}
