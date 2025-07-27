using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RinhaDeBackend.Infra.Migrations
{
    /// <inheritdoc />
    public partial class teste : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "processedOnFallback",
                table: "Payments",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "processedOnFallback",
                table: "Payments");
        }
    }
}
