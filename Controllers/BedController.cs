using Microsoft.AspNetCore.Mvc;
using GuestHouseBookingApplication.Interfaces;
using GuestHouseBookingApplication.Models.DTOs;
using System.Threading.Tasks;
using System;

[ApiController]
[Route("api/[controller]")]
public class BedController : ControllerBase
{
    private readonly IBedService _bedService;

    public BedController(IBedService bedService)
    {
        _bedService = bedService;
    }

    [HttpGet("room/{roomId}")]
    public async Task<IActionResult> GetBedsByRoom(int roomId) =>
        Ok(await _bedService.GetBedsByRoomIdAsync(roomId));

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(int id)
    {
        var result = await _bedService.GetByIdAsync(id);
        if (result == null) return NotFound();
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateBedDto dto) =>
        Ok(await _bedService.CreateAsync(dto));

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateBedDto dto)
    {
        var updated = await _bedService.UpdateAsync(id, dto);
        if (!updated) return NotFound();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _bedService.DeleteAsync(id);
        if (!deleted) return NotFound();
        return NoContent();
    }

    [HttpGet("room/{roomId}/available")]
    public async Task<IActionResult> GetAvailableBeds(int roomId, [FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
    {
        var beds = await _bedService.GetAvailableBedsForDateRangeAsync(roomId, startDate, endDate);
        return Ok(beds);
    }
}