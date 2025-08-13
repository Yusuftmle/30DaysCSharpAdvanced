using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Day06.API.Migrations
{
    /// <inheritdoc />
    public partial class mig_2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "id",
                table: "BlogPosts",
                newName: "Id");

            migrationBuilder.AddColumn<DateTime>(
                name: "createdDate",
                table: "Reviews",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "createdDate",
                table: "Outlines",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "createdDate",
                table: "ContentBlocks",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "createdDate",
                table: "Reviews");

            migrationBuilder.DropColumn(
                name: "createdDate",
                table: "Outlines");

            migrationBuilder.DropColumn(
                name: "createdDate",
                table: "ContentBlocks");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "BlogPosts",
                newName: "id");
        }
    }
}
