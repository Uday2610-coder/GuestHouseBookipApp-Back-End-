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
    public class RoomService : IRoomService
    {
        private readonly GuestHouseBookingDbContext _context;

        public RoomService(GuestHouseBookingDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<RoomDto>> GetRoomsByGuestHouseIdAsync(int guestHouseId)
        {
            return await _context.Rooms
                .Where(r => r.GuestHouseId == guestHouseId)
                .Select(r => new RoomDto { Id = r.Id, Name = r.Name, GuestHouseId = r.GuestHouseId })
                .ToListAsync();
        }

        public async Task<RoomDto> GetByIdAsync(int id)
        {
            var room = await _context.Rooms.FindAsync(id);
            if (room == null) return null;
            return new RoomDto { Id = room.Id, Name = room.Name, GuestHouseId = room.GuestHouseId };
        }

        public async Task<RoomDto> CreateAsync(CreateRoomDto dto)
        {
            var entity = new Room { Name = dto.Name, GuestHouseId = dto.GuestHouseId };
            _context.Rooms.Add(entity);
            await _context.SaveChangesAsync();
            return new RoomDto { Id = entity.Id, Name = entity.Name, GuestHouseId = entity.GuestHouseId };
        }

        public async Task<bool> UpdateAsync(int id, UpdateRoomDto dto)
        {
            var room = await _context.Rooms.FindAsync(id);
            if (room == null) return false;
            room.Name = dto.Name;
            room.GuestHouseId = dto.GuestHouseId;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var room = await _context.Rooms.FindAsync(id);
            if (room == null) return false;
            _context.Rooms.Remove(room);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}