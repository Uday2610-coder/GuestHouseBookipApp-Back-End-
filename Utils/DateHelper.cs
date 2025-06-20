using System;

namespace GuestHouseBookingApplication.Utils
{
    public static class DateHelper
    {
        public static bool IsValidDateRange(DateTime start, DateTime end)
        {
            return start <= end && start >= DateTime.Today;
        }

        public static int GetNumberOfDays(DateTime start, DateTime end)
        {
            return (end - start).Days + 1;
        }
    }
}