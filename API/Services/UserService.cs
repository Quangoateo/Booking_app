using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using NetUtility;
using BookingApp.Constants;
using BookingApp.Data;
using BookingApp.DTO;
using BookingApp.Helpers;
using BookingApp.Models;
using BookingApp.Services.Base;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Syncfusion.JavaScript;
using Syncfusion.JavaScript.DataSources;
using OfficeOpenXml.FormulaParsing.LexicalAnalysis;
using BookingApp.DTO.Filter;
using BookingApp.DTO.auth;

namespace BookingApp.Services
{
    public interface IUserService : IServiceBase<User, UserDto>
    {
        Task<OperationResult> CheckExistLdap(string ldapName);
        Task<OperationResult> LoginAsync(UserForLoginDto userForLoginDto);
        Task<List<UserDto>> SearchUser(UserFilter userFilter);
        Task<OperationResult> UpdateRole(int id, int roleId);
        Task<UserWithPermission> GetUserWithPermissionAsync(int id);
    }
    public class UserService : ServiceBase<User, UserDto>, IUserService
    {
        private readonly IRepositoryBase<User> _repo;
        private readonly IRepositoryBase<Role> _roleRepo;
        private readonly IRepositoryBase<RolePermission> _rolePermissionRepo;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly MapperConfiguration _configMapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IWebHostEnvironment _currentEnvironment;
        private readonly IAuthService _authService;
        public UserService(
            IRepositoryBase<User> repo,
            IRepositoryBase<Role> roleRepo,
            IRepositoryBase<RolePermission> rolePermissionRepo,
            IUnitOfWork unitOfWork,
            IMapper mapper,
            MapperConfiguration configMapper,
            IHttpContextAccessor httpContextAccessor,
            IWebHostEnvironment currentEnvironment,
            IAuthService authService
            )
            : base(repo, unitOfWork, mapper, configMapper)
        {
            _repo = repo;
            _roleRepo = roleRepo;
            _rolePermissionRepo = rolePermissionRepo;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _configMapper = configMapper;
            _httpContextAccessor = httpContextAccessor;
            _currentEnvironment = currentEnvironment;
            _authService = authService;
        }

        public override async Task<OperationResult> UpdateAsync(UserDto model)
        {
            try
            {
                var item = await _repo.FindByIDAsync(model.ID);
                _repo.Update(item);

                await _unitOfWork.SaveChangeAsync();
                var result = _mapper.Map<UserDto>(item);
                operationResult = new OperationResult
                {
                    StatusCode = HttpStatusCode.OK,
                    Message = MessageReponse.UpdateSuccess,
                    Success = true,
                    Data = result
                };
            }
            catch (Exception ex)
            {
                operationResult = ex.GetMessageError();
            }
            return operationResult;
        }
        public async Task<OperationResult> CheckExistLdap(string ldapName)
        {
            var item = await _repo.FindAll(x => x.LdapName == ldapName && x.Status).FirstOrDefaultAsync();
            if (item != null)
            {
                return new OperationResult { 
                    StatusCode = HttpStatusCode.OK, 
                    Message = "The account already existed!", 
                    Success = true ,
                    Data = item };
            }
            operationResult = new OperationResult
            {
                StatusCode = HttpStatusCode.OK,
                Success = false,
                Data = item
            };
            return operationResult;
        }
        public override async Task<List<UserDto>> GetAllAsync()
        {
            var query =  _repo.FindAll(x => x.Status);
            var roleQuery =   _roleRepo.FindAll(x => x.Status);
            var user2role = query.Join(roleQuery, x => x.RoleId, y => y.ID, (x, y) => new { x, y }).
                DefaultIfEmpty() // left join
                .Select(x => new UserDto
                {
                    ID = x.x.ID,
                    Name = x.x.Name,
                    Email = x.x.Email,
                    Department = x.x.Department,
                    RoleId = x.x.ID,
                    RoleName = x.y.RoleName,
                    StaffCode = x.x.StaffCode,
                    LdapName = x.x.LdapName,
                    Password = x.x.Password,
                    CreateDate = x.x.CreateDate,
                    CreateBy = x.x.CreateBy,
                    LdapLogin = x.x.LdapLogin,
                    Status = x.x.Status,
                    Guid = x.x.Guid
                }).ToList();
            return user2role;
        }
        public override async Task<OperationResult> AddAsync(UserDto model)
        {
            try
            {
                var item = _mapper.Map<User>(model);
                item.Status = true;
                item.Password = item.Password.ToSha512();
                item.LdapLogin = false;
                item.Guid = Guid.NewGuid().ToString("N") + DateTime.Now.ToString("ssff").ToUpper();
                //item.StaffCode = false;
                _repo.Add(item);
                await _unitOfWork.SaveChangeAsync();
                operationResult = new OperationResult
                {
                    StatusCode = HttpStatusCode.OK,
                    Message = MessageReponse.AddSuccess,
                    Success = true,
                    Data = item
                };
            }
            catch (Exception ex)
            {
                operationResult = ex.GetMessageError();
            }

            return operationResult;
        }
        public async Task<UserWithPermission> GetUserWithPermissionAsync(int id)
        {
            try { 
                var user = await _repo.FindByIDAsync(id);
                var role = await _roleRepo.FindAll(x => x.ID == user.RoleId).FirstOrDefaultAsync();
                var permissions = await _rolePermissionRepo.FindAll(x => x.ID == user.RoleId).ToListAsync();
                var userWithPermission = new UserWithPermission
                {
                    ID = user.ID,
                    Name = user.Name,
                    Email = user.Email,
                    Department = user.Department,
                    RoleName = role.RoleName,
                    StaffCode = user.StaffCode,
                    LdapName = user.LdapName,
                    Permissions = permissions.Count > 0 ? _mapper.Map<List<RolePermissionDto>>(permissions) : null
                };
                return userWithPermission;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public override async Task<OperationResult> DeleteAsync(int id)
        {
            try
            {
                var item = await _repo.FindByIDAsync(id);
                _repo.Remove(item);
                await _unitOfWork.SaveChangeAsync();
                operationResult = new OperationResult
                {
                    StatusCode = HttpStatusCode.OK,
                    Message = MessageReponse.DeleteSuccess,
                    Success = true,
                    Data = item
                };
            }
            catch (Exception ex)
            {
                operationResult = ex.GetMessageError();
            }

            return operationResult;
        }
        
        public async Task<List<UserDto>> SearchUser(UserFilter userFilter)
        {
            //var query = _repo.FindAll().ProjectTo<UserDto>(_configMapper);
            var query = _repo.FindAll(x => x.Status);
            var roleQuery = _roleRepo.FindAll(x => x.Status);
            var user2role = query.Join(roleQuery, x => x.RoleId, y => y.ID, (x, y) => new { x, y }).
                DefaultIfEmpty() // left join
                .Select(x => new UserDto
                {
                    ID = x.x.ID,
                    Name = x.x.Name,
                    Email = x.x.Email,
                    Department = x.x.Department,
                    RoleId = x.x.ID,
                    RoleName = x.y.RoleName,
                    StaffCode = x.x.StaffCode,
                    LdapName = x.x.LdapName,
                    Password = x.x.Password,
                    CreateDate = x.x.CreateDate,
                    CreateBy = x.x.CreateBy,
                    LdapLogin = x.x.LdapLogin,
                    Status = x.x.Status,
                    Guid = x.x.Guid
                });
            if (!string.IsNullOrWhiteSpace(userFilter.Department))
            {
                user2role = user2role.Where(x => x.Department == userFilter.Department);
            }
            if (!string.IsNullOrWhiteSpace(userFilter.RoleName))
            {
                user2role = user2role.Where(x => x.RoleName == userFilter.RoleName);
            }
            if (userFilter.LDapLogin)
            {
                user2role = user2role.Where(x => x.LdapLogin == userFilter.LDapLogin);
            }
            var data = await user2role.ToListAsync();
            return data;
        }
        public async Task<OperationResult> UpdateRole(int id, int roleId)
        {
            try
            {
                var item = await _repo.FindByIDAsync(id);
                item.RoleId = roleId;
                _repo.Update(item);
                await _unitOfWork.SaveChangeAsync();
                var result = _mapper.Map<UserDto>(item);
                operationResult = new OperationResult
                {
                    StatusCode = HttpStatusCode.OK,
                    Message = MessageReponse.UpdateSuccess,
                    Success = true,
                    Data = result
                };
            }
            catch (Exception ex)
            {
                operationResult = ex.GetMessageError();
            }
            return operationResult;
        }
        public async Task<OperationResult> LoginAsync(UserForLoginDto userForLoginDto)
        {
            return await _authService.NewLoginAsync(userForLoginDto);
        }
    }
}
