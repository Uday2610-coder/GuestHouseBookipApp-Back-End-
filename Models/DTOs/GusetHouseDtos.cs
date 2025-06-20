
namespace GuestHouseBookingApplication.Models.DTOs
{
    public class GuestHouseDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
    }

    public class CreateGuestHouseDto
    {
        public string Name { get; set; }
        public string Address { get; set; }
    }

    public class UpdateGuestHouseDto
    {
        public string Name { get; set; }
        public string Address { get; set; }
    }
}