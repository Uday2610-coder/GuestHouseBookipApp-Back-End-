namespace GuestHouseBookingApplication.Utils
{
    public static class BookingStatusHelper
    {
        public const string Pending = "Pending";
        public const string Approved = "Approved";
        public const string Rejected = "Rejected";

        public static bool IsFinalStatus(string status)
        {
            return status == Approved || status == Rejected;
        }
    }
}