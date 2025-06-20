namespace GuestHouseBookingApplication.Models.DTOs
{
    public class BedDto
    {
        public int Id { get; set; }
        public string BedNumber { get; set; }
        public int RoomId { get; set; }
    }

    public class CreateBedDto
    {
        public string BedNumber { get; set; }
        public int RoomId { get; set; }
    }

    public class UpdateBedDto
    {
        public string BedNumber { get; set; }
        public int RoomId { get; set; }
    }
}