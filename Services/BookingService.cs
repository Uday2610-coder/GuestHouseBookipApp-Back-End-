using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GuestHouseBookingApplication.Data;
using GuestHouseBookingApplication.Interfaces;
using GuestHouseBookingApplication.Models.DTOs;
using GuestHouseBookingApplication.Models.Entities;
using GuestHouseBookingApplication.Models.Enums;
using Microsoft.EntityFrameworkCore;

namespace GuestHouseBookingApplication.Services
{
    public class BookingService : IBookingService
    {
        private readonly GuestHouseBookingDbContext _context;

        public BookingService(GuestHouseBookingDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<BookingResponseDto>> GetAllAsync()
        {
            try
            {
                return await _context.Bookings
                    .Include(b => b.GuestHouse)
                    .Include(b => b.Room)
                    .Include(b => b.Bed)
                    .Include(b => b.User)
                    .Select(b => new BookingResponseDto
                    {
                        Id = b.Id,
                        GuestHouseId = b.GuestHouseId,
                        GuestHouseName = b.GuestHouse.Name,
                        RoomId = b.RoomId,
                        RoomName = b.Room.Name,
                        BedId = b.BedId,
                        BedNumber = b.Bed.BedNumber,
                        UserId = b.UserId,
                        UserFullName = b.User.FullName,
                        StartDate = b.StartDate,
                        EndDate = b.EndDate,
                        Status = b.Status.ToString(),
                        AdminRemarks = b.AdminRemarks,
                        CreatedAt = b.CreatedAt,
                        DecisionDate = b.DecisionDate,
                        Address = b.Address,
                        // Dynamic price calculation
                        Price = b.Price ?? CalculatePrice(b.RoomId, b.StartDate, b.EndDate), // Use stored price if present
                        Gender = b.Gender, // NEW
                        Purpose = b.Purpose // <-- ADD THIS LINE

                    }).ToListAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR] GetAllAsync failed: {ex.Message}");
                Console.WriteLine($"[STACKTRACE] {ex.StackTrace}");
                if (ex.InnerException != null)
                {
                    Console.WriteLine($"[INNER EXCEPTION] {ex.InnerException.Message}");
                    Console.WriteLine($"[INNER STACKTRACE] {ex.InnerException.StackTrace}");
                }
                throw new Exception("Unable to retrieve bookings.", ex);
            }
        }
        // Helper method to calculate the price
        public static decimal CalculatePrice(int roomId, DateTime? startDate, DateTime? endDate)
        {
            if (startDate == null || endDate == null)
            {
                throw new ArgumentNullException("StartDate or EndDate cannot be null.");
            }

            decimal pricePerDay = roomId switch
            {
                1 => 5000, // Deluxe Room
                2 => 3000, // Standard Room
                3 => 8000, // Suite
                _ => 0
            };



            int days = (endDate - startDate)?.Days ?? 0;
            if (days <= 0)
            {
                throw new ArgumentException("EndDate must be greater than StartDate.");
            }
            return pricePerDay * days;
        }

        public async Task<IEnumerable<BookingResponseDto>> GetByUserIdAsync(int userId)
        {
            return await _context.Bookings
                .Include(b => b.GuestHouse)
                .Include(b => b.Room)
                .Include(b => b.Bed)
                .Include(b => b.User)
                .Where(b => b.UserId == userId)
                .Select(b => new BookingResponseDto
                {
                    Id = b.Id,
                    GuestHouseId = b.GuestHouseId,
                    GuestHouseName = b.GuestHouse.Name,
                    RoomId = b.RoomId,
                    RoomName = b.Room.Name,
                    BedId = b.BedId,
                    BedNumber = b.Bed.BedNumber,
                    UserId = b.UserId,
                    UserFullName = b.User.FullName,
                    StartDate = b.StartDate,
                    EndDate = b.EndDate,
                    Status = b.Status.ToString(),
                    AdminRemarks = b.AdminRemarks,
                    CreatedAt = b.CreatedAt,
                    DecisionDate = b.DecisionDate,
                    Address = b.Address,
                    Price = b.Price ?? CalculatePrice(b.RoomId, b.StartDate, b.EndDate),
                    Gender = b.Gender,
                    Purpose = b.Purpose // <-- ADD THIS LINE
                }).ToListAsync();
        }

        public async Task<BookingResponseDto> GetByIdAsync(int id)
        {
            var b = await _context.Bookings
                .Include(x => x.GuestHouse)
                .Include(x => x.Room)
                .Include(x => x.Bed)
                .Include(x => x.User)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (b == null) return null;

            return new BookingResponseDto
            {
                Id = b.Id,
                GuestHouseId = b.GuestHouseId,
                GuestHouseName = b.GuestHouse.Name,
                RoomId = b.RoomId,
                RoomName = b.Room.Name,
                BedId = b.BedId,
                BedNumber = b.Bed.BedNumber,
                UserId = b.UserId,
                UserFullName = b.User.FullName,
                StartDate = b.StartDate,
                EndDate = b.EndDate,
                Status = b.Status.ToString(),
                AdminRemarks = b.AdminRemarks,
                CreatedAt = b.CreatedAt,
                DecisionDate = b.DecisionDate,
                Address = b.Address,
                Price = b.Price ?? CalculatePrice(b.RoomId, b.StartDate, b.EndDate),
                Gender = b.Gender, // NEW
                Purpose = b.Purpose // <-- ADD THIS LINE

            };
        }

        public async Task<BookingResponseDto> CreateAsync(BookingRequestDto dto, int userId)
        {
            try
            {
                var booking = new Booking
                {
                    GuestHouseId = dto.GuestHouseId,
                    RoomId = dto.RoomId,
                    BedId = dto.BedId,
                    UserId = userId,
                    StartDate = dto.StartDate,
                    EndDate = dto.EndDate,
                    Status = BookingStatus.Pending,
                    CreatedAt = DateTime.UtcNow,
                    Address = dto.Address,
                    Price = dto.Price,
                    Gender = dto.Gender,
                    Purpose = dto.Purpose,
                    AdminRemarks = ""
                };
                if (booking.Price == null)
                    booking.Price = CalculatePrice(dto.RoomId, dto.StartDate, dto.EndDate);

                _context.Bookings.Add(booking);
                await _context.SaveChangesAsync();

                return await GetByIdAsync(booking.Id);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR] CreateAsync failed: {ex.Message}");
                Console.WriteLine($"[STACKTRACE] {ex.StackTrace}");
                if (ex.InnerException != null)
                    Console.WriteLine($"[INNER EXCEPTION] {ex.InnerException.Message}");
                throw;
            }
        }

        public async Task<bool> ApproveOrRejectAsync(ApproveRejectBookingDto dto, int adminId)
        {
            var booking = await _context.Bookings.FindAsync(dto.BookingId);
            if (booking == null || booking.Status != BookingStatus.Pending) return false;

            if (Enum.TryParse<BookingStatus>(dto.Status, out var status))
            {
                booking.Status = status;
                booking.AdminRemarks = dto.AdminRemarks;
                booking.DecisionDate = DateTime.UtcNow;
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<bool> UpdateAsync(int id, BookingUpdateDto dto)
        {
            var booking = await _context.Bookings.FindAsync(id);
            if (booking == null) return false;

            var guestHouse = await _context.GuestHouses.FirstOrDefaultAsync(g => g.Name == dto.GuestHouseName);
            if (guestHouse == null)
            {
                Console.WriteLine($"Guest house not found: {dto.GuestHouseName}");
                return false;
            }

            var room = await _context.Rooms.FirstOrDefaultAsync(r => r.Name == dto.RoomName && r.GuestHouseId == guestHouse.Id);
            if (room == null)
            {
                Console.WriteLine($"Room not found: {dto.RoomName} in GuestHouseId: {guestHouse.Id}");
                return false;
            }

            var bed = await _context.Beds.FirstOrDefaultAsync(b => b.BedNumber == dto.BedNumber && b.RoomId == room.Id);
            if (bed == null)
            {
                Console.WriteLine($"Bed not found: {dto.BedNumber} in RoomId: {room.Id}");
                return false;
            }

            booking.GuestHouseId = guestHouse.Id;
            booking.RoomId = room.Id;
            booking.BedId = bed.Id;
            booking.StartDate = dto.StartDate;
            booking.EndDate = dto.EndDate;
            booking.Address = dto.Address;
            booking.Gender = dto.Gender; // NEW
            booking.Price = dto.Price;
            booking.Purpose = dto.Purpose; // <-- ADD THIS LINE (in UpdateAsync)

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var booking = await _context.Bookings.FindAsync(id);
            if (booking == null) return false;

            _context.Bookings.Remove(booking);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<BookingResponseDto>> GetReportBookingsAsync(
    string address,
    string guestHouseName,
    string status,
    DateTime? startDate,
    DateTime? endDate
)
        {
            Console.WriteLine($"[DEBUG] GetReportBookingsAsync called with:");
            Console.WriteLine($"  address: {address}");
            Console.WriteLine($"  guestHouseName: {guestHouseName}");
            Console.WriteLine($"  status: {status}");
            Console.WriteLine($"  startDate: {startDate}");
            Console.WriteLine($"  endDate: {endDate}");

            var query = _context.Bookings
                .Include(b => b.GuestHouse)
                .Include(b => b.Room)
                .Include(b => b.Bed)
                .Include(b => b.User)
                .AsQueryable();

            Console.WriteLine($"[DEBUG] Initial bookings count: {query.Count()}");

            if (!string.IsNullOrEmpty(address))
            {
                query = query.Where(b => !string.IsNullOrEmpty(b.Address) && b.Address.ToLower().Contains(address.ToLower()));
                Console.WriteLine($"[DEBUG] After address filter: {query.Count()}");
            }

            if (!string.IsNullOrEmpty(guestHouseName))
            {
                query = query.Where(b => !string.IsNullOrEmpty(b.GuestHouse.Name) && b.GuestHouse.Name.ToLower() == guestHouseName.ToLower());
                Console.WriteLine($"[DEBUG] After guestHouseName filter: {query.Count()}");
            }

            if (!string.IsNullOrEmpty(status))
            {
                if (Enum.TryParse(typeof(BookingStatus), status, true, out var parsedStatus))
                {
                    var statusValue = (BookingStatus)parsedStatus;
                    query = query.Where(b => b.Status == statusValue);
                    Console.WriteLine($"[DEBUG] After status filter: {query.Count()}");
                }
                else
                {
                    Console.WriteLine($"[DEBUG] Status '{status}' could not be parsed to BookingStatus enum.");
                }
            }

            if (startDate.HasValue && endDate.HasValue)
            {
                query = query.Where(b =>
                    b.StartDate >= startDate.Value && b.EndDate <= endDate.Value
                );
                Console.WriteLine($"[DEBUG] After startDate && endDate filter: {query.Count()}");
            }
            else if (startDate.HasValue)
            {
                query = query.Where(b => b.EndDate >= startDate.Value);
                Console.WriteLine($"[DEBUG] After startDate filter: {query.Count()}");
            }
            else if (endDate.HasValue)
            {
                query = query.Where(b => b.StartDate <= endDate.Value);
                Console.WriteLine($"[DEBUG] After endDate filter: {query.Count()}");
            }

            var results = await query.Select(b => new BookingResponseDto
            {
                Id = b.Id,
                GuestHouseId = b.GuestHouseId,
                GuestHouseName = b.GuestHouse.Name,
                RoomId = b.RoomId,
                RoomName = b.Room.Name,
                BedId = b.BedId,
                BedNumber = b.Bed.BedNumber,
                UserId = b.UserId,
                UserFullName = b.User.FullName,
                StartDate = b.StartDate,
                EndDate = b.EndDate,
                Status = b.Status.ToString(),
                AdminRemarks = b.AdminRemarks,
                CreatedAt = b.CreatedAt,
                DecisionDate = b.DecisionDate,
                Address = b.Address,
                Price = b.Price ?? CalculatePrice(b.RoomId, b.StartDate, b.EndDate),
                Gender = b.Gender,
                Purpose = b.Purpose
            }).ToListAsync();

            Console.WriteLine($"[DEBUG] Final results count: {results.Count}");

            return results;
        }

    }
}