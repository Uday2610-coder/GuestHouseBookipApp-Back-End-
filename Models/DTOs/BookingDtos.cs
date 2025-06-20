using System;

namespace GuestHouseBookingApplication.Models.DTOs
{
    public class BookingRequestDto
    {
        public int GuestHouseId { get; set; }
        public int RoomId { get; set; }
        public int BedId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Address { get; set; }
        public decimal? Price { get; set; } // NEW: User-supplied price
        public string Gender { get; set; } // NEW
        public string? Purpose { get; set; } // <-- ADD THIS LINE



    }

    public class BookingResponseDto
    {
        public int Id { get; set; }
        public int GuestHouseId { get; set; }
        public string GuestHouseName { get; set; }
        public int RoomId { get; set; }
        public string RoomName { get; set; }
        public int BedId { get; set; }
        public string BedNumber { get; set; }
        public int UserId { get; set; }
        public string UserFullName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Status { get; set; }
        public string AdminRemarks { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? DecisionDate { get; set; }
        public decimal? Price { get; set; } // NEW: User-supplied price
        public string Address { get; set; }
        public string Gender { get; set; } // NEW
        public string? Purpose { get; set; } // <-- ADD THIS LINE


    }

    public class ApproveRejectBookingDto
    {
        public int BookingId { get; set; }
        public string Status { get; set; } // "Approved" or "Rejected"
        public string AdminRemarks { get; set; }
    }
}