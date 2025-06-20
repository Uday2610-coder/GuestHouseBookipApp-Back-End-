using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GuestHouseBookingApplication.Models.Entities
{
    public class Bed
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string BedNumber { get; set; }

        // Foreign key
        [ForeignKey("Room")]
        public int RoomId { get; set; }
        public virtual Room Room { get; set; }

        public virtual ICollection<Booking> Bookings { get; set; }
    }
}
