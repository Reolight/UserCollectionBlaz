using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UserCollectionBlaz.Migrations
{
    public partial class stage17 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Likes_AppUser",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_AppUser",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "AppUser",
                table: "AspNetUsers");

            migrationBuilder.CreateTable(
                name: "AppUserLike",
                columns: table => new
                {
                    AppUser = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LikedById = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppUserLike", x => new { x.AppUser, x.LikedById });
                    table.ForeignKey(
                        name: "FK_AppUserLike_AspNetUsers_LikedById",
                        column: x => x.LikedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AppUserLike_Likes_AppUser",
                        column: x => x.AppUser,
                        principalTable: "Likes",
                        principalColumn: "Position",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AppUserLike_LikedById",
                table: "AppUserLike",
                column: "LikedById");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AppUserLike");

            migrationBuilder.AddColumn<string>(
                name: "AppUser",
                table: "AspNetUsers",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_AppUser",
                table: "AspNetUsers",
                column: "AppUser");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Likes_AppUser",
                table: "AspNetUsers",
                column: "AppUser",
                principalTable: "Likes",
                principalColumn: "Position");
        }
    }
}
