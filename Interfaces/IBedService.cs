using System.Collections.Generic;
using System.Threading.Tasks;
using GuestHouseBookingApplication.Models.DTOs;

namespace GuestHouseBookingApplication.Interfaces
{
    public interface IBedService
    {
        Task<IEnumerable<BedDto>> GetBedsByRoomIdAsync(int roomId);
        Task<BedDto> GetByIdAsync(int id);
        Task<BedDto> CreateAsync(CreateBedDto dto);
        Task<bool> UpdateAsync(int id, UpdateBedDto dto);
        Task<bool> DeleteAsync(int id);
        Task<IEnumerable<BedDto>> GetAvailableBedsForDateRangeAsync(int roomId, System.DateTime startDate, System.DateTime endDate);
    }
}