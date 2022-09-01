using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UserCollectionBlaz.Migrations
{
    public partial class stage13 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Likes_LikePosition",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_LikePosition",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "LikePosition",
                table: "AspNetUsers");

            migrationBuilder.CreateTable(
                name: "AppUserLike",
                columns: table => new
                {
                    LikedById = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LikesPosition = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppUserLike", x => new { x.LikedById, x.LikesPosition });
                    table.ForeignKey(
                        name: "FK_AppUserLike_AspNetUsers_LikedById",
                        column: x => x.LikedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AppUserLike_Likes_LikesPosition",
                        column: x => x.LikesPosition,
                        principalTable: "Likes",
                        principalColumn: "Position",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AppUserLike_LikesPosition",
                table: "AppUserLike",
                column: "LikesPosition");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AppUserLike");

            migrationBuilder.AddColumn<string>(
                name: "LikePosition",
                table: "AspNetUsers",
                type: "nvarchar(450)",
                nullable: true);

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
        }
    }
}
