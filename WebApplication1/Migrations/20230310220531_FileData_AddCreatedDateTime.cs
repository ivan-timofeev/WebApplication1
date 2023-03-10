using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApplication1.Migrations
{
    public partial class FileData_AddCreatedDateTime : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDateTimeUtc",
                table: "FilesData",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedDateTimeUtc",
                table: "FilesData",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "FilesData",
                type: "boolean",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedDateTimeUtc",
                table: "FilesData",
                type: "timestamp with time zone",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedDateTimeUtc",
                table: "FilesData");

            migrationBuilder.DropColumn(
                name: "DeletedDateTimeUtc",
                table: "FilesData");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "FilesData");

            migrationBuilder.DropColumn(
                name: "UpdatedDateTimeUtc",
                table: "FilesData");
        }
    }
}
