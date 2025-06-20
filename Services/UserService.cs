using System;
using System.Threading.Tasks;
using GuestHouseBookingApplication.Interfaces;
using GuestHouseBookingApplication.Models.DTOs;
using GuestHouseBookingApplication.Models.Entities;
using GuestHouseBookingApplication.Utils;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;

namespace GuestHouseBookingApplication.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly INotificationService _notificationService;
        private readonly IConfiguration _configuration;

        public UserService(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            INotificationService notificationService,
            IConfiguration configuration)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _notificationService = notificationService;
            _configuration = configuration;
        }
        public async Task<RegisterResultDto> RegisterAsync(UserRegisterDto dto)
        {
            // Check if email is already taken
            var existingUser = await _userManager.FindByEmailAsync(dto.Email);
            if (existingUser != null)
            {
                return new RegisterResultDto
                {
                    Success = false,
                    ErrorMessage = "Email is already registered."
                };
            }

            // Create a new ApplicationUser
            var user = new ApplicationUser
            {
                UserName = dto.Email,
                Email = dto.Email,
                FullName = dto.FullName
            };

            // Create user with provided password
            var result = await _userManager.CreateAsync(user, dto.Password);

            if (!result.Succeeded)
            {
                // Collect error messages
                var errors = string.Join("; ", result.Errors.Select(e => e.Description));
                return new RegisterResultDto
                {
                    Success = false,
                    ErrorMessage = errors
                };
            }

            // Optionally, assign default role
            await _userManager.AddToRoleAsync(user, "User");

            // Optionally, send notification/email
            // await _notificationService.SendEmailAsync(dto.Email, "Welcome!", "Thank you for registering!");

            return new RegisterResultDto
            {
                Success = true
            };
        }

        public async Task<string> LoginAsync(UserLoginDto dto)
        {
            try
            {
                // Normalize email for case-insensitive comparison
                string normalizedEmail = dto.Email.ToUpperInvariant();
                Console.WriteLine($"Attempting login for email: {dto.Email}");

                // Step 1: Find user by email
                var user = await _userManager.FindByEmailAsync(dto.Email);
                if (user == null)
                {
                    Console.WriteLine($"User not found: {dto.Email}");
                    return null;
                }

                // Step 2: Verify password
                var result = await _signInManager.CheckPasswordSignInAsync(user, dto.Password, false);
                if (!result.Succeeded)
                {
                    Console.WriteLine($"Password verification failed for user: {dto.Email}");
                    return null;
                }

                Console.WriteLine($"Login successful for user: {dto.Email}");

                // Step 3: Fetch roles
                var roles = await _userManager.GetRolesAsync(user);
                if (roles == null || !roles.Any())
                {
                    Console.WriteLine($"No roles found for user {user.Email}. Assigning default role 'User'.");
                    roles = new[] { "User" }; // fallback/default role
                }
                else
                {
                    Console.WriteLine($"Roles for user {user.Email}: {string.Join(", ", roles)}");
                }

                // Step 4: Generate JWT token
                string secretKey = _configuration["Jwt:Key"];
                if (string.IsNullOrEmpty(secretKey))
                {
                    Console.WriteLine("JWT Secret Key is missing in configuration.");
                    throw new InvalidOperationException("JWT Secret Key is not configured.");
                }

                string token = JwtTokenGenerator.GenerateToken(
                    user.Id.ToString(),
                    user.Email,
                    roles,    // Pass the entire collection!
                    secretKey
                );
                Console.WriteLine($"Generated JWT Token for user {user.Email}.");
                return token;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred during login: {ex.Message}");
                return null;
            }
        }

        public async Task<bool> ChangePasswordAsync(int userId, ChangePasswordDto dto)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null) return false;
            var result = await _userManager.ChangePasswordAsync(user, dto.OldPassword, dto.NewPassword);
            return result.Succeeded;
        }

        public async Task<bool> ForgotPasswordAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null || !user.EmailConfirmed) return false;
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);

            // Send password reset email using INotificationService/EmailSender
            string resetLink = $"https://yourdomain.com/reset-password?email={email}&token={System.Net.WebUtility.UrlEncode(token)}";
            await _notificationService.SendEmailAsync(
                email,
                "Password Reset Request",
                $"Please reset your password using this link: {resetLink}"
            );
            return true;
        }

        public async Task<bool> ResetPasswordAsync(ResetPasswordDto dto)
        {
            var user = await _userManager.FindByEmailAsync(dto.Email);
            if (user == null) return false;
            var result = await _userManager.ResetPasswordAsync(user, dto.Token, dto.NewPassword);
            return result.Succeeded;
        }

        public async Task<bool> IsEmailTakenAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            return user != null;
        }

        public async Task<int?> GetUserIdByEmailAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            return user?.Id;
        }

    }
}
