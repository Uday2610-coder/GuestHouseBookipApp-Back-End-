using System.Threading.Tasks;
using GuestHouseBookingApplication.Models.DTOs;

namespace GuestHouseBookingApplication.Interfaces
{
    public interface IAuditLogService
    {
        Task LogAsync(AuditLogDto dto);
    }
}