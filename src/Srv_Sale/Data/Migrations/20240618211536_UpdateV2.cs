using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Srv_Sale.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateV2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Brand",
                table: "Sales");

            migrationBuilder.DropColumn(
                name: "Model",
                table: "Sales");

            migrationBuilder.RenameColumn(
                name: "Make",
                table: "Items",
                newName: "Brand");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Brand",
                table: "Items",
                newName: "Make");

            migrationBuilder.AddColumn<string>(
                name: "Brand",
                table: "Sales",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Model",
                table: "Sales",
                type: "text",
                nullable: true);
        }
    }
}
