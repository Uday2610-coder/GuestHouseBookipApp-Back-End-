using System;
using System.IO;

namespace GuestHouseBookingApplication.Utils
{
    public static class AuditLogger
    {
        private static readonly string logFilePath = "Logs/audit.log";

        public static void Log(string user, string action, string details)
        {
            Directory.CreateDirectory("Logs");
            var log = $"{DateTime.UtcNow:u} | User: {user} | Action: {action} | Details: {details}";
            File.AppendAllText(logFilePath, log + Environment.NewLine);
        }
    }
}