using Microsoft.AspNetCore.Mvc;
using GuestHouseBookingApplication.Interfaces;
using System.Threading.Tasks;

[ApiController]
[Route("api/[controller]")]
public class NotificationController : ControllerBase
{
    private readonly INotificationService _notificationService;

    public NotificationController(INotificationService notificationService)
    {
        _notificationService = notificationService;
    }

   
    [HttpPost("user/{userId}")]
    public async Task<IActionResult> SendToUser(int userId, [FromBody] string message)
    {
        await _notificationService.SendBookingNotificationAsync(userId, message);
        return Ok();
    }

    [HttpPost("admin")]
    public async Task<IActionResult> SendToAdmins([FromBody] string message)
    {
        await _notificationService.SendAdminNotificationAsync(message);
        return Ok();
    }
}