using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GuestHouseBookingApplication.Migrations
{
    /// <inheritdoc />
    public partial class AddPurposeToBooking : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Purpose",
                table: "Bookings",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Purpose",
                table: "Bookings");
        }
    }
}
