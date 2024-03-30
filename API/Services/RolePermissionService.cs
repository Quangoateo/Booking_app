using AutoMapper;
using BookingApp.Data;
using BookingApp.DTO;
using BookingApp.Models;
using BookingApp.Services.Base;
using System.Threading.Tasks;

namespace BookingApp.Services
{
    public interface IRolePermissionService : IServiceBase<RolePermission, RolePermissionDto>
    {
    }
    public class RolePermissionService : ServiceBase<RolePermission, RolePermissionDto>, IRolePermissionService
    {
        private readonly RepositoryBase<RolePermission> _repo;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly MapperConfiguration _configMapper;

        public RolePermissionService(
            IRepositoryBase<RolePermission> repo, 
            IUnitOfWork unitOfWork, 
            IMapper mapper, 
            MapperConfiguration configMapper) 
            : base(repo, unitOfWork, mapper, configMapper)
        {
            _repo = (RepositoryBase<RolePermission>)repo;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _configMapper = configMapper;
        }

    }
}
