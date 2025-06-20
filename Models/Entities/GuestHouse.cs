using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace GuestHouseBookingApplication.Models.Entities
{
    public class GuestHouse
    {
        
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string Address { get; set; }

      

        // Navigation property
        public virtual ICollection<Room> Rooms { get; set; }
        public virtual ICollection<Booking> Bookings { get; set; }
    }
}

