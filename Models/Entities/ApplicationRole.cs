using Microsoft.AspNetCore.Identity;

namespace GuestHouseBookingApplication.Models.Entities
{
    public class ApplicationRole : IdentityRole<int>
    {
        public string Description { get; set; }
    }
}
