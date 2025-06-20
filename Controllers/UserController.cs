using Microsoft.AspNetCore.Mvc;
using GuestHouseBookingApplication.Interfaces;
using GuestHouseBookingApplication.Models.DTOs;
using System.Threading.Tasks;
using System;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] UserRegisterDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await _userService.RegisterAsync(dto);
        if (!result.Success)
            return BadRequest(result.ErrorMessage);

        return Ok("Registration successful.");
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] UserLoginDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var token = await _userService.LoginAsync(dto);
        if (token == null)
        {
            Console.WriteLine("Login failed: Invalid credentials.");
            return Unauthorized();
        }
        Console.WriteLine($"Login successful for user: {dto.Email}");
        return Ok(new { Token = token });
    }

    [HttpPost("change-password/{userId}")]
    public async Task<IActionResult> ChangePassword(int userId, [FromBody] ChangePasswordDto dto)
    {
        var result = await _userService.ChangePasswordAsync(userId, dto);
        if (!result) return BadRequest("Password change failed.");
        return Ok("Password changed successfully.");
    }

    [HttpPost("forgot-password")]
    public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordDto dto)
    {
        try
        {
            var result = await _userService.ForgotPasswordAsync(dto.Email);
            if (!result)
                return BadRequest("Email not found or not confirmed.");

            return Ok("Password reset email sent.");
        }
        catch (Exception ex)
        {
            // Log the exception (to file, DB, or at least console)
            Console.WriteLine($"[ForgotPassword] Error: {ex}");

            // For debugging, return the error. For production, just return a generic message.
            return StatusCode(500, $"Internal server error: {ex.Message}");
            // Or for production: return StatusCode(500, "An unexpected error occurred.");
        }
    }

    [HttpPost("reset-password")]
    public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDto dto)
    {
        var result = await _userService.ResetPasswordAsync(dto);
        if (!result) return BadRequest("Reset failed.");
        return Ok("Password reset successful.");
    }
}