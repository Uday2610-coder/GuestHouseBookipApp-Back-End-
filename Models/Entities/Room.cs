using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GuestHouseBookingApplication.Models.Entities
{
    public class Room
    {
        
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        // Foreign key
        [ForeignKey("GuestHouse")]
        public int GuestHouseId { get; set; }
        public virtual GuestHouse GuestHouse { get; set; }

        public virtual ICollection<Bed> Beds { get; set; }
        public virtual ICollection<Booking> Bookings { get; set; }
    }
}

