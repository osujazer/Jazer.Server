using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Jazer.Server.Migrations
{
    /// <inheritdoc />
    public partial class AddCountryCodeToUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "country_code",
                table: "users",
                type: "char(2)",
                nullable: false,
                defaultValue: "XX");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "country_code",
                table: "users");
        }
    }
}
