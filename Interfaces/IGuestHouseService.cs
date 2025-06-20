using System.Collections.Generic;
using System.Threading.Tasks;
using GuestHouseBookingApplication.Models.DTOs;

namespace GuestHouseBookingApplication.Interfaces
{
    public interface IGuestHouseService
    {
        Task<IEnumerable<GuestHouseDto>> GetAllAsync();
        Task<GuestHouseDto> GetByIdAsync(int id);
        Task<GuestHouseDto> CreateAsync(CreateGuestHouseDto dto);
        Task<bool> UpdateAsync(int id, UpdateGuestHouseDto dto);
        Task<bool> DeleteAsync(int id);
    }
}