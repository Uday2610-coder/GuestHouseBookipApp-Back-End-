using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GuestHouseBookingApplication.Migrations
{
    /// <inheritdoc />
    public partial class RenameLocationToAddressInBooking : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Location",
                table: "Bookings",
                newName: "Address");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Address",
                table: "Bookings",
                newName: "Location");
        }
    }
}
