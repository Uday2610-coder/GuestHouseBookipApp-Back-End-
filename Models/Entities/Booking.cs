using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using GuestHouseBookingApplication.Models.Enums;


namespace GuestHouseBookingApplication.Models.Entities
{
    public class Booking
    {
        [Key]
        public int Id { get; set; }

        // Foreign key to User
        [ForeignKey("User")]
        public int UserId { get; set; }
        public virtual ApplicationUser User { get; set; }

        // Foreign key to GuestHouse
        [ForeignKey("GuestHouse")]
        public int GuestHouseId { get; set; }
        public virtual GuestHouse GuestHouse { get; set; }

        // Foreign key to Room
        [ForeignKey("Room")]
        public int RoomId { get; set; }
        public virtual Room Room { get; set; }

        // Foreign key to Bed
        [ForeignKey("Bed")]
        public int BedId { get; set; }
        public virtual Bed Bed { get; set; }

        [Required]
        public DateTime StartDate { get; set; }
        [Required]
        public DateTime EndDate { get; set; }

        [Required]
        public BookingStatus Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? DecisionDate { get; set; }


        public string AdminRemarks { get; set; }

        public string Address { get; set; }

        public decimal? Price { get; set; }

        public string Gender { get; set; }

        public string? Purpose { get; set; }


    }
}
