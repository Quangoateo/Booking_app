using AutoMapper;
using BookingApp.Data;
using BookingApp.DTO;
using BookingApp.Models;
using BookingApp.Services.Base;
using System.Threading.Tasks;

namespace BookingApp.Services
{
    public interface IFacilityService:IServiceBase<Facility, FacilityDto>
    {

    }
    public class FacilityService : ServiceBase<Facility, FacilityDto>, IFacilityService
    {
        private readonly IRepositoryBase<Facility> _repo;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly MapperConfiguration _configMapper;
        public FacilityService (
                       IRepositoryBase<Facility> repo,
                                  IUnitOfWork unitOfWork,
                                             IMapper mapper,
                                                        MapperConfiguration configMapper
                   ) : base(repo, unitOfWork, mapper, configMapper)
        {
            _repo = repo;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _configMapper = configMapper;
        }
    }
}
