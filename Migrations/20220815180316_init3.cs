using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UserCollectionBlaz.Migrations
{
    public partial class init3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "AutorId",
                table: "comments",
                newName: "AutorName");

            migrationBuilder.AddColumn<DateTime>(
                name: "PostedTime",
                table: "comments",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PostedTime",
                table: "comments");

            migrationBuilder.RenameColumn(
                name: "AutorName",
                table: "comments",
                newName: "AutorId");
        }
    }
}
