using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GuestHouseBookingApplication.Migrations
{
    /// <inheritdoc />
    public partial class AddLocationToBooking_Fix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Location",
                table: "Bookings",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Location",
                table: "Bookings");
        }
    }
}
