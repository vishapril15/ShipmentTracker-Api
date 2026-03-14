using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Secure_Shipment_Tracker.Migrations
{
    /// <inheritdoc />
    public partial class NewShipmentNumber : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ShipmentNumber",
                table: "Shipments",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ShipmentNumber",
                table: "Shipments");
        }
    }
}
