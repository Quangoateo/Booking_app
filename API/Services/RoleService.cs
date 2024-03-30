using AutoMapper;
using BookingApp.Data;
using BookingApp.DTO;
using BookingApp.Models;
using BookingApp.Services.Base;

namespace BookingApp.Services
{
    public interface IRoleService : IServiceBase<Role,RoleDto>
    {
    }
    public class RoleService : ServiceBase<Role,RoleDto>, IRoleService
    {
        private readonly IRepositoryBase<Role> _repo;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly MapperConfiguration _configMapper;
        public RoleService(
                       IRepositoryBase<Role> repo,
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
    }
   
}
