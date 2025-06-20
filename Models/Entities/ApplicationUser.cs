using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GuestHouseBookingApplication.Models.Entities
{
    public class ApplicationUser : IdentityUser<int>
    {
        [Required]
        public string FullName { get; set; }

        public virtual ICollection<Booking> Bookings { get; set; }
        public virtual ICollection<Notification> Notifications { get; set; }
        public virtual ICollection<AuditLog> AuditLogs { get; set; }
    }
}
