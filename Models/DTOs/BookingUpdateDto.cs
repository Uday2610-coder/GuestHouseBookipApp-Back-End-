using System;

namespace GuestHouseBookingApplication.Models.DTOs
{
    public class BookingUpdateDto
    {
        public string GuestHouseName { get; set; }
        public string RoomName { get; set; }
        public string BedNumber { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Address { get; set; }
        public string Gender { get; set; }
        public decimal? Price { get; set; } 
        public string? Purpose { get; set; } 

    }
}
