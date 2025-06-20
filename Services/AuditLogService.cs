using System.Threading.Tasks;
using GuestHouseBookingApplication.Data;
using GuestHouseBookingApplication.Interfaces;
using GuestHouseBookingApplication.Models.DTOs;
using GuestHouseBookingApplication.Models.Entities;

namespace GuestHouseBookingApplication.Services
{
    public class AuditLogService : IAuditLogService
    {
        private readonly GuestHouseBookingDbContext _context;

        public AuditLogService(GuestHouseBookingDbContext context)
        {
            _context = context;
        }

        public async Task LogAsync(AuditLogDto dto)
        {
            var auditLog = new AuditLog
            {
                UserId = dto.UserId,
                Action = dto.Action,
                EntityName = dto.EntityName,
                EntityId = dto.EntityId,
                Changes = dto.Changes,
                Timestamp = dto.Timestamp
            };
            _context.AuditLogs.Add(auditLog);
            await _context.SaveChangesAsync();
        }
    }
}