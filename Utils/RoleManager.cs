namespace GuestHouseBookingApplication.Utils
{
    public static class RoleManager
    {
        public static bool IsAdmin(string role) => role == "Admin";
        public static bool IsUser(string role) => role == "User";
        
    }
}