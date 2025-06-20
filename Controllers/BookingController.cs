using Microsoft.AspNetCore.Mvc;
using GuestHouseBookingApplication.Models.DTOs;
using GuestHouseBookingApplication.Interfaces;
using System.Threading.Tasks;
using System;
using System.Text;

[ApiController]
[Route("api/[controller]")]
public class BookingController : ControllerBase
{
    private readonly IBookingService _bookingService;

    public BookingController(IBookingService bookingService)
    {
        _bookingService = bookingService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        try
        {
            var result = await _bookingService.GetAllAsync();
            return Ok(result);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[ERROR] GetAll failed: {ex.Message}");
            Console.WriteLine($"[STACKTRACE] {ex.StackTrace}");
            if (ex.InnerException != null)
            {
                Console.WriteLine($"[INNER EXCEPTION] {ex.InnerException.Message}");
            }

            // Return a detailed error response
            return StatusCode(500, "Internal Server Error: An error occurred while fetching" );
        }
    }

    [HttpGet("user/{userId}")]
    public async Task<IActionResult> GetByUser(int userId) =>
        Ok(await _bookingService.GetByUserIdAsync(userId));

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(int id)
    {
        var result = await _bookingService.GetByIdAsync(id);
        if (result == null) return NotFound();
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] BookingRequestDto dto, [FromQuery] int userId) =>
        Ok(await _bookingService.CreateAsync(dto, userId));

    [HttpPost("approve-reject")]
    public async Task<IActionResult> ApproveOrReject([FromBody] ApproveRejectBookingDto dto, [FromQuery] int adminId)
    {
        var result = await _bookingService.ApproveOrRejectAsync(dto, adminId);
        if (!result) return BadRequest("Invalid operation or booking not found.");
        return Ok();
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateBooking(int id, [FromBody] BookingUpdateDto dto)
    {
        var result = await _bookingService.UpdateAsync(id, dto);
        if (!result) return NotFound();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _bookingService.DeleteAsync(id);
        if (!result) return NotFound();
        return NoContent();
    }

    [HttpGet("export")]
    public async Task<IActionResult> Export(
       [FromQuery] string address = null,
       [FromQuery] string guestHouseName = null,
       [FromQuery] string status = null,
       [FromQuery] DateTime? startDate = null,
       [FromQuery] DateTime? endDate = null)
    {
        try
        {
            var result = await _bookingService.GetAllAsync();

            // Apply filters (same as above)
            if (!string.IsNullOrEmpty(address))
                result = result.Where(b => !string.IsNullOrEmpty(b.Address) && b.Address.Contains(address, StringComparison.OrdinalIgnoreCase));
            if (!string.IsNullOrEmpty(guestHouseName))
                result = result.Where(b => !string.IsNullOrEmpty(b.GuestHouseName) && b.GuestHouseName.Equals(guestHouseName, StringComparison.OrdinalIgnoreCase));
            if (!string.IsNullOrEmpty(status))
                result = result.Where(b => !string.IsNullOrEmpty(b.Status) && b.Status.Equals(status, StringComparison.OrdinalIgnoreCase));
            if (startDate.HasValue)
                result = result.Where(b => b.StartDate != null && b.StartDate >= startDate.Value);
            if (endDate.HasValue)
                result = result.Where(b => b.EndDate != null && b.EndDate <= endDate.Value);

            // Create CSV
            var csv = new StringBuilder();
            csv.AppendLine("Sr no.,Employee Name,Location,Guest House,Room,Bed,Check-In,Check-Out,Status");
            int i = 1;
            foreach (var b in result)
            {
                string checkIn = b.StartDate.ToString("yyyy-MM-dd");
                string checkOut = b.EndDate.ToString("yyyy-MM-dd");
                csv.AppendLine($"{i++},\"{b.UserFullName}\",\"{b.Address}\",\"{b.GuestHouseName}\",\"{b.RoomName}\",\"{b.BedNumber}\",\"{b.StartDate}\",\"{b.EndDate}\",\"{b.Status}\"");
            }
            var bytes = Encoding.UTF8.GetBytes(csv.ToString());
            return File(bytes, "text/csv", "report.csv");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[ERROR] Export failed: {ex.Message}");
            return StatusCode(500, "Internal Server Error: Could not export data.");
        }

        string FormatDate(DateTime? dt) => dt.HasValue ? dt.Value.ToString("yyyy-MM-dd") : "";

    }

    [HttpGet("report")]
    public async Task<IActionResult> GetReportBookings(
    [FromQuery] string address = null,
    [FromQuery] string guestHouseName = null,
    [FromQuery] string status = null,
    [FromQuery] DateTime? startDate = null,
    [FromQuery] DateTime? endDate = null)
    {
        try
        {
            var result = await _bookingService.GetReportBookingsAsync(address, guestHouseName, status, startDate, endDate);
            return Ok(result);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[ERROR] GetReportBookings failed: {ex.Message}");
            return StatusCode(500, "Internal Server Error: Could not fetch filtered bookings.");
        }
    }
}