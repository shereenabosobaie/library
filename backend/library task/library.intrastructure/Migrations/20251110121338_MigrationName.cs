using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace library.intrastructure.Migrations
{
    /// <inheritdoc />
    public partial class MigrationName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "members",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Email", "passwordHas" },
                values: new object[] { "admin", "AQAAAAIAAYagAAAAEAUoBi0U2jjozI6ABQdVjD91aSw0mDfe1aE+JrnTAkF+ClBZj2uM/aM9AqqU9J8PIA==" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "members",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Email", "passwordHas" },
                values: new object[] { "admin@library.com", "now" });
        }
    }
}
