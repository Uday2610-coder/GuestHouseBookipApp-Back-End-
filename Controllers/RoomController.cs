using Microsoft.AspNetCore.Mvc;
using GuestHouseBookingApplication.Interfaces;
using GuestHouseBookingApplication.Models.DTOs;
using System.Threading.Tasks;

[ApiController]
[Route("api/[controller]")]
public class RoomController : ControllerBase
{
    private readonly IRoomService _roomService;

    public RoomController(IRoomService roomService)
    {
        _roomService = roomService;
    }

    [HttpGet("guesthouse/{guestHouseId}")]
    public async Task<IActionResult> GetRoomsByGuestHouse(int guestHouseId) =>
        Ok(await _roomService.GetRoomsByGuestHouseIdAsync(guestHouseId));

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(int id)
    {
        var result = await _roomService.GetByIdAsync(id);
        if (result == null) return NotFound();
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateRoomDto dto) =>
        Ok(await _roomService.CreateAsync(dto));

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateRoomDto dto)
    {
        var updated = await _roomService.UpdateAsync(id, dto);
        if (!updated) return NotFound();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _roomService.DeleteAsync(id);
        if (!deleted) return NotFound();
        return NoContent();
    }
}