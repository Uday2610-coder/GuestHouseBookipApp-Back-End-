using Microsoft.AspNetCore.Mvc;
using GuestHouseBookingApplication.Data;
using GuestHouseBookingApplication.Models.DTOs;
using System.Linq;
using System.Threading.Tasks;

[ApiController]
[Route("api/[controller]")]
public class AuditLogController : ControllerBase
{
    private readonly GuestHouseBookingDbContext _context;

    public AuditLogController(GuestHouseBookingDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public IActionResult Get()
    {
        var logs = _context.AuditLogs
            .OrderByDescending(a => a.Timestamp)
            .Select(a => new AuditLogDto
            {
                Id = a.Id,
                UserId = a.UserId,
                Action = a.Action,
                EntityName = a.EntityName,
                EntityId = a.EntityId,
                Changes = a.Changes,
                Timestamp = a.Timestamp
            })
            .ToList();

        return Ok(logs);
    }
}