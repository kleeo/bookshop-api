using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bookshop.Migrations
{
    /// <inheritdoc />
    public partial class FixBookingAddressPropName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DeliveryAdderss",
                table: "Bookings",
                newName: "DeliveryAddress");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DeliveryAddress",
                table: "Bookings",
                newName: "DeliveryAdderss");
        }
    }
}
