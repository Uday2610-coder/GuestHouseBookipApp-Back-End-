using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GuestHouseBookingApplication.Data;
using GuestHouseBookingApplication.Interfaces;
using GuestHouseBookingApplication.Models.DTOs;
using GuestHouseBookingApplication.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace GuestHouseBookingApplication.Services
{
    public class BedService : IBedService
    {
        private readonly GuestHouseBookingDbContext _context;

        public BedService(GuestHouseBookingDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<BedDto>> GetBedsByRoomIdAsync(int roomId)
        {
            return await _context.Beds
                .Where(b => b.RoomId == roomId)
                .Select(b => new BedDto { Id = b.Id, BedNumber = b.BedNumber, RoomId = b.RoomId })
                .ToListAsync();
        }

        public async Task<BedDto> GetByIdAsync(int id)
        {
            var bed = await _context.Beds.FindAsync(id);
            if (bed == null) return null;
            return new BedDto { Id = bed.Id, BedNumber = bed.BedNumber, RoomId = bed.RoomId };
        }

        public async Task<BedDto> CreateAsync(CreateBedDto dto)
        {
            var entity = new Bed { BedNumber = dto.BedNumber, RoomId = dto.RoomId };
            _context.Beds.Add(entity);
            await _context.SaveChangesAsync();
            return new BedDto { Id = entity.Id, BedNumber = entity.BedNumber, RoomId = entity.RoomId };
        }

        public async Task<bool> UpdateAsync(int id, UpdateBedDto dto)
        {
            var bed = await _context.Beds.FindAsync(id);
            if (bed == null) return false;
            bed.BedNumber = dto.BedNumber;
            bed.RoomId = dto.RoomId;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var bed = await _context.Beds.FindAsync(id);
            if (bed == null) return false;
            _context.Beds.Remove(bed);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<BedDto>> GetAvailableBedsForDateRangeAsync(int roomId, System.DateTime startDate, System.DateTime endDate)
        {
            // Beds not booked in the given date range
            var bookedBedIds = await _context.Bookings
                .Where(b => b.RoomId == roomId &&
                            b.Status != Models.Enums.BookingStatus.Rejected &&
                            (startDate < b.EndDate && endDate > b.StartDate))
                .Select(b => b.BedId)
                .ToListAsync();

            return await _context.Beds
                .Where(b => b.RoomId == roomId && !bookedBedIds.Contains(b.Id))
                .Select(b => new BedDto { Id = b.Id, BedNumber = b.BedNumber, RoomId = b.RoomId })
                .ToListAsync();
        }
    }
}