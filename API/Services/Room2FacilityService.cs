using AutoMapper;
using BookingApp.Data;
using BookingApp.DTO;
using BookingApp.Models;
using BookingApp.Services.Base;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BookingApp.Services
{
    public interface IRoom2FacilityService : IServiceBase<Room2Facility, Room2FacilityDto>
    {
    }
    public class Room2FacilityService : ServiceBase<Room2Facility, Room2FacilityDto>, IRoom2FacilityService
    {
        private readonly IRepositoryBase<Room2Facility> _repo;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly MapperConfiguration _configMapper;
        public Room2FacilityService(
            IRepositoryBase<Room2Facility> repo,
            IUnitOfWork unitOfWork,
            IMapper mapper,
            MapperConfiguration configMapper) : 
            base(repo, unitOfWork, mapper, configMapper)
        {
            _repo = repo;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _configMapper = configMapper;
        }
        //public override Task<List<Room2FacilityDto>> GetAllAsync() { 
        //}
    }
}
