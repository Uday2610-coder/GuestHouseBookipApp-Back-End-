using System;
using System.Collections.Generic;
using System.Linq;

namespace GuestHouseBookingApplication.Utils
{
    public static class AvailabilityChecker
    {
        // This method checks if a resource (bed/room) is available for the given date range
        public static bool IsAvailable(IEnumerable<(DateTime Start, DateTime End)> bookings, DateTime requestedStart, DateTime requestedEnd)
        {
            return !bookings.Any(b => requestedStart < b.End && requestedEnd > b.Start);
        }
    }
}