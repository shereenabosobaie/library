using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace library.intrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Addti : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "books",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "books");
        }
    }
}
