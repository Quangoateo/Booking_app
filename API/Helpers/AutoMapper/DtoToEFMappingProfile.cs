using AutoMapper;
using BookingApp.Data;
using BookingApp.DTO;
using BookingApp.DTO.auth;
using BookingApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookingApp.Helpers.AutoMapper
{
    public class DtoToEFMappingProfile : Profile
    {
        public DtoToEFMappingProfile()
        {
            CreateMap<BuildingDto, Building>();
            CreateMap<CampusDto, Campus>();
            CreateMap<UserDto, User>();
            CreateMap<RoomDto, Room>();
            CreateMap<FloorDto, Floor>();
            CreateMap<BookingDto, Booking>();
            CreateMap<LogDto, Log>();
            CreateMap<FacilityDto, Facility>();
            CreateMap<Room2FacilityDto, Room2Facility>();
            CreateMap<RoleDto, Role>();
            CreateMap<RolePermissionDto, RolePermission>();
        }
    }
}
