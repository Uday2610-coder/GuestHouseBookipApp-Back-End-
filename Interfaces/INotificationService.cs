using System.Threading.Tasks;

namespace GuestHouseBookingApplication.Interfaces
{
    public interface INotificationService
    {
        Task SendEmailAsync(string toEmail, string subject, string body);
        Task SendBookingNotificationAsync(int userId, string message);
        Task SendAdminNotificationAsync(string message);
    }
}