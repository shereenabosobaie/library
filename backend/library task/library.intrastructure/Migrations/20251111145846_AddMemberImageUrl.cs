using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace library.intrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddMemberImageUrl : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                table: "members",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                table: "books",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "addedtime",
                table: "books",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AlterColumn<double>(
                name: "Rate",
                table: "BookRates",
                type: "float",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.UpdateData(
                table: "members",
                keyColumn: "Id",
                keyValue: 1,
                column: "ImageUrl",
                value: null);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageUrl",
                table: "members");

            migrationBuilder.DropColumn(
                name: "ImageUrl",
                table: "books");

            migrationBuilder.DropColumn(
                name: "addedtime",
                table: "books");

            migrationBuilder.AlterColumn<int>(
                name: "Rate",
                table: "BookRates",
                type: "int",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float");
        }
    }
}
