using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Automatis.Migrations
{
    /// <inheritdoc />
    public partial class RenameYearToYears : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Year",
                table: "Cars",
                newName: "Years");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Years",
                table: "Cars",
                newName: "Year");
        }
    }
}
