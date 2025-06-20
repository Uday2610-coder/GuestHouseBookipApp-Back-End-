using System.Text.RegularExpressions;

namespace GuestHouseBookingApplication.Utils
{
    public static class ValidationHelper
    {
        public static bool IsValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email)) return false;
            var pattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
            return Regex.IsMatch(email, pattern);
        }

        public static bool IsValidPhone(string phone)
        {
            var pattern = @"^\+?\d{10,15}$";
            return Regex.IsMatch(phone, pattern);
        }
    }
}