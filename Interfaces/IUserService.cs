using System.Threading.Tasks;
using GuestHouseBookingApplication.Models.DTOs;

namespace GuestHouseBookingApplication.Interfaces
{
    public interface IUserService
    {
        Task<string> LoginAsync(UserLoginDto dto);
        Task<bool> ChangePasswordAsync(int userId, ChangePasswordDto dto);
        Task<bool> ForgotPasswordAsync(string email);
        Task<bool> ResetPasswordAsync(ResetPasswordDto dto);
        Task<bool> IsEmailTakenAsync(string email);
        Task<int?> GetUserIdByEmailAsync(string email);
        Task<RegisterResultDto> RegisterAsync(UserRegisterDto dto);
    }
}