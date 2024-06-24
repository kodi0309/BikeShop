using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Srv_Sale.Data.Migrations
{
    /// <inheritdoc />
    public partial class Restart2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "StartPrice",
                table: "Sales",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }
    }
}
