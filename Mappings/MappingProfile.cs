using AutoMapper;
using GuestHouseBookingApplication.Models.Entities;
using GuestHouseBookingApplication.Models.DTOs;

namespace GuestHouseBookingApplication.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<GuestHouse, GuestHouseDto>().ReverseMap();
            CreateMap<CreateGuestHouseDto, GuestHouse>();
            CreateMap<UpdateGuestHouseDto, GuestHouse>();

            CreateMap<Room, RoomDto>().ReverseMap();
            CreateMap<CreateRoomDto, Room>();
            CreateMap<UpdateRoomDto, Room>();

            CreateMap<Bed, BedDto>().ReverseMap();
            CreateMap<CreateBedDto, Bed>();
            CreateMap<UpdateBedDto, Bed>();

            CreateMap<Booking, BookingResponseDto>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()))
                .ForMember(dest => dest.GuestHouseName, opt => opt.MapFrom(src => src.GuestHouse.Name))
                .ForMember(dest => dest.RoomName, opt => opt.MapFrom(src => src.Room.Name))
                .ForMember(dest => dest.BedNumber, opt => opt.MapFrom(src => src.Bed.BedNumber))
                .ForMember(dest => dest.UserFullName, opt => opt.MapFrom(src => src.User.FullName));
            CreateMap<BookingRequestDto, Booking>();
            CreateMap<ApproveRejectBookingDto, Booking>();

            CreateMap<Notification, NotificationDto>().ReverseMap();
            CreateMap<AuditLog, AuditLogDto>().ReverseMap();
        }
    }
}