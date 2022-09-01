using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UserCollectionBlaz.Migrations
{
    public partial class stage14 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AppUserLike_Likes_LikesPosition",
                table: "AppUserLike");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AppUserLike",
                table: "AppUserLike");

            migrationBuilder.DropIndex(
                name: "IX_AppUserLike_LikesPosition",
                table: "AppUserLike");

            migrationBuilder.RenameColumn(
                name: "LikesPosition",
                table: "AppUserLike",
                newName: "AppUser");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AppUserLike",
                table: "AppUserLike",
                columns: new[] { "AppUser", "LikedById" });

            migrationBuilder.CreateIndex(
                name: "IX_AppUserLike_LikedById",
                table: "AppUserLike",
                column: "LikedById");

            migrationBuilder.AddForeignKey(
                name: "FK_AppUserLike_Likes_AppUser",
                table: "AppUserLike",
                column: "AppUser",
                principalTable: "Likes",
                principalColumn: "Position",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AppUserLike_Likes_AppUser",
                table: "AppUserLike");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AppUserLike",
                table: "AppUserLike");

            migrationBuilder.DropIndex(
                name: "IX_AppUserLike_LikedById",
                table: "AppUserLike");

            migrationBuilder.RenameColumn(
                name: "AppUser",
                table: "AppUserLike",
                newName: "LikesPosition");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AppUserLike",
                table: "AppUserLike",
                columns: new[] { "LikedById", "LikesPosition" });

            migrationBuilder.CreateIndex(
                name: "IX_AppUserLike_LikesPosition",
                table: "AppUserLike",
                column: "LikesPosition");

            migrationBuilder.AddForeignKey(
                name: "FK_AppUserLike_Likes_LikesPosition",
                table: "AppUserLike",
                column: "LikesPosition",
                principalTable: "Likes",
                principalColumn: "Position",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
