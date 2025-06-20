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
    public class GuestHouseService : IGuestHouseService
    {
        private readonly GuestHouseBookingDbContext _context;

        public GuestHouseService(GuestHouseBookingDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<GuestHouseDto>> GetAllAsync()
        {
            return await _context.GuestHouses
                .Select(gh => new GuestHouseDto
                {
                    Id = gh.Id,
                    Name = gh.Name,
                    Address = gh.Address
                }).ToListAsync();
        }

        public async Task<GuestHouseDto> GetByIdAsync(int id)
        {
            var gh = await _context.GuestHouses.FindAsync(id);
            if (gh == null) return null;
            return new GuestHouseDto { Id = gh.Id, Name = gh.Name, Address = gh.Address };
        }

        public async Task<GuestHouseDto> CreateAsync(CreateGuestHouseDto dto)
        {
            var entity = new GuestHouse { Name = dto.Name, Address = dto.Address };
            _context.GuestHouses.Add(entity);
            await _context.SaveChangesAsync();
            return new GuestHouseDto { Id = entity.Id, Name = entity.Name, Address = entity.Address };
        }

        public async Task<bool> UpdateAsync(int id, UpdateGuestHouseDto dto)
        {
            var gh = await _context.GuestHouses.FindAsync(id);
            if (gh == null) return false;
            gh.Name = dto.Name;
            gh.Address = dto.Address;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var gh = await _context.GuestHouses.FindAsync(id);
            if (gh == null) return false;
            _context.GuestHouses.Remove(gh);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}