using System.Collections.Generic;
using System.Threading.Tasks;
using GuestHouseBookingApplication.Models.DTOs;

namespace GuestHouseBookingApplication.Interfaces
{
    public interface IRoomService
    {
        Task<IEnumerable<RoomDto>> GetRoomsByGuestHouseIdAsync(int guestHouseId);
        Task<RoomDto> GetByIdAsync(int id);
        Task<RoomDto> CreateAsync(CreateRoomDto dto);
        Task<bool> UpdateAsync(int id, UpdateRoomDto dto);
        Task<bool> DeleteAsync(int id);
    }
}