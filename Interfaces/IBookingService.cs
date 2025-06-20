using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using GuestHouseBookingApplication.Models.DTOs;
using System;

namespace GuestHouseBookingApplication.Interfaces
{
    public interface IBookingService
    {
        Task<IEnumerable<BookingResponseDto>> GetAllAsync();
        Task<IEnumerable<BookingResponseDto>> GetByUserIdAsync(int userId);
        Task<BookingResponseDto> GetByIdAsync(int id);
        Task<BookingResponseDto> CreateAsync(BookingRequestDto dto, int userId);
        Task<bool> ApproveOrRejectAsync(ApproveRejectBookingDto dto, int adminId);
        Task<bool> UpdateAsync(int id, BookingUpdateDto dto);
        Task<bool> DeleteAsync(int id);

        Task<IEnumerable<BookingResponseDto>> GetReportBookingsAsync(
          string address,
          string guestHouseName,
          string status,
          DateTime? startDate,
          DateTime? endDate
      );
    }
} 