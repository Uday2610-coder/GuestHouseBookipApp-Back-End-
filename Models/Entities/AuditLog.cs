using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GuestHouseBookingApplication.Models.Entities
{
    public class AuditLog
    {
        [Key]
        public int Id { get; set; }

        // Foreign key to User (nullable)
        [ForeignKey("User")]
        public int? UserId { get; set; }
        public virtual ApplicationUser User { get; set; }

        [Required]
        public string Action { get; set; }
        [Required]
        public string EntityName { get; set; }
        public int? EntityId { get; set; }
        public string Changes { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
