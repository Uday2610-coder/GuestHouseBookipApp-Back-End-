using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;

namespace GuestHouseBookingApplication.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AdminController : ControllerBase
    {
        // This endpoint is protected by the AdminPolicy
        [Authorize(Policy = "AdminPolicy")]
        [HttpGet("admin-only")]
        public IActionResult AdminOnlyEndpoint()
        {
            Console.WriteLine("AdminOnlyEndpoint reached");
            return Ok("Welcome Admin! You have access to this endpoint.");
        }

        // Public endpoint for testing purposes
        [HttpGet("public")]
        public IActionResult PublicEndpoint()
        {
            return Ok("This is a public endpoint. No authorization required.");
        }
    }
}




