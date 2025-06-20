using Microsoft.AspNetCore.Mvc;
using GuestHouseBookingApplication.Interfaces;
using GuestHouseBookingApplication.Models.DTOs;
using System.Threading.Tasks;

[ApiController]
[Route("api/[controller]")]
public class GuestHouseController : ControllerBase
{
    private readonly IGuestHouseService _guestHouseService;

    public GuestHouseController(IGuestHouseService guestHouseService)
    {
        _guestHouseService = guestHouseService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll() =>
        Ok(await _guestHouseService.GetAllAsync());

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(int id)
    {
        var result = await _guestHouseService.GetByIdAsync(id);
        if (result == null) return NotFound();
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateGuestHouseDto dto) =>
        Ok(await _guestHouseService.CreateAsync(dto));

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateGuestHouseDto dto)
    {
        var updated = await _guestHouseService.UpdateAsync(id, dto);
        if (!updated) return NotFound();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _guestHouseService.DeleteAsync(id);
        if (!deleted) return NotFound();
        return NoContent();
    }
}