using System.Threading.Tasks;
using GuestHouseBookingApplication.Interfaces;
using GuestHouseBookingApplication.Utils;
using Microsoft.Extensions.Configuration;

namespace GuestHouseBookingApplication.Services
{
    public class NotificationService : INotificationService
    {
        private readonly IConfiguration _configuration;

        public NotificationService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task SendEmailAsync(string toEmail, string subject, string body)
        {
            // Fetch SMTP config from appsettings.json
            string smtpHost = _configuration["Smtp:Host"];
            int smtpPort = int.Parse(_configuration["Smtp:Port"]);
            string smtpUser = _configuration["Smtp:User"];
            string smtpPass = _configuration["Smtp:Pass"];

            var emailSender = new EmailSender(smtpHost, smtpPort, smtpUser, smtpPass);
            await emailSender.SendEmailAsync(toEmail, subject, body);
        }

        public async Task SendBookingNotificationAsync(int userId, string message)
        {
            // TODO: Lookup user email from userId 
            string userEmail = await GetUserEmailByIdAsync(userId);

            // Send the booking notification email
            await SendEmailAsync(userEmail, "Booking Notification", message);
        }

        public async Task SendAdminNotificationAsync(string message)
        {
            // TODO: Get admin email(s) from configuration or database
            string adminEmail = _configuration["Admin:Email"];
            await SendEmailAsync(adminEmail, "Admin Notification", message);
        }

        // Example placeholder for getting user email by ID
        private Task<string> GetUserEmailByIdAsync(int userId)
        {
            // Replace with real lookup
            return Task.FromResult("user@example.com");
        }
    }
}