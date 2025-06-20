namespace GuestHouseBookingApplication.Models.DTOs
{
    public class RoomDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int GuestHouseId { get; set; }
    }

    public class CreateRoomDto
    {
        public string Name { get; set; }
        public int GuestHouseId { get; set; }
    }

    public class UpdateRoomDto
    {
        public string Name { get; set; }
        public int GuestHouseId { get; set; }
    }
}