using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using BookingApp.Constants;
using BookingApp.Data;
using BookingApp.DTO;
using BookingApp.Helpers;
using BookingApp.Models;
using BookingApp.Services.Base;
using Syncfusion.JavaScript;
using Syncfusion.JavaScript.DataSources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.IO;
using Microsoft.AspNetCore.Http;
using NetUtility;
using Microsoft.AspNetCore.Hosting;

namespace BookingApp.Services
{
    public interface IBuildingService : IServiceBase<Building, BuildingDto>
    {
        Task<object> LoadData(DataManager data, string farmGuid);
        //Task<object> GetAudit(object id);
        Task<OperationResult> AddFormAsync(BuildingDto model);
        Task<object> DeleteUploadFile(int key);
        Task<object> GetSitesByAccount();
        Task<object> CheckRoom();

    }
    public class BuildingService : ServiceBase<Building, BuildingDto>, IBuildingService
    {
        private readonly IRepositoryBase<Building> _repo;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly MapperConfiguration _configMapper;
        private readonly IWebHostEnvironment _currentEnvironment;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public BuildingService(
            IRepositoryBase<Building> repo,
            IUnitOfWork unitOfWork,
            IMapper mapper,
            IWebHostEnvironment currentEnvironment,
            IHttpContextAccessor httpContextAccessor,
            MapperConfiguration configMapper
            )
            : base(repo, unitOfWork, mapper, configMapper)
        {
            _repo = repo;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _configMapper = configMapper;
            _currentEnvironment = currentEnvironment;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<object> GetSitesByAccount()
        {
            throw new NotImplementedException();
            //var accessToken = _httpContextAccessor.HttpContext.Request.Headers["Authorization"];
            //int accountID = JWTExtensions.GetDecodeTokenByID(accessToken);
        }
        public override async Task<OperationResult> AddAsync(BuildingDto model)
        {
            try
            {
                var item = _mapper.Map<Building>(model);
                item.Status = true;
                item.BuildingGuid = Guid.NewGuid().ToString("N") + DateTime.Now.ToString("ssff").ToUpper();
                _repo.Add(item);
                await _unitOfWork.SaveChangeAsync();

                operationResult = new OperationResult
                {
                    StatusCode = HttpStatusCode.OK,
                    Message = MessageReponse.AddSuccess,
                    Success = true,
                    Data = model
                };
            }
            catch (Exception ex)
            {
                operationResult = ex.GetMessageError();
            }
            return operationResult;
        }
        public override async Task<OperationResult> AddRangeAsync(List<BuildingDto> model)
        {
            var item = _mapper.Map<List<Building>>(model);
            _repo.AddRange(item);
            try
            {
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
        public override async Task<List<BuildingDto>> GetAllAsync()
        {
            var query = _repo.FindAll(x => x.Status).ProjectTo<BuildingDto>(_configMapper);

            var data = await query.ToListAsync();
            return data;

        }
        public override async Task<OperationResult> DeleteAsync(int id)
        {
            var item = _repo.FindByID(id);
            //item.CancelFlag = "Y";
            item.Status = false;
            _repo.Update(item);
            try
            {
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

        //public override async Task<PagedList<BuildingDto>> GetWithPaginationsAsync(PaginationParams param)
        //{
        //    var lists = _repo.FindAll().ProjectTo<BuildingDto>(_configMapper).OrderByDescending(x => x.ID);

        //    return await PagedList<BuildingDto>.CreateAsync(lists, param.PageNumber, param.PageSize);
        //}
        public async Task<object> LoadData(DataManager data, string farmGuid)
        {
            throw new NotImplementedException();
        }
        //public async Task<object> GetAudit(object id)
        //{
        //    throw new NotImplementedException();
        //}
        public async Task<OperationResult> CheckExistSitename(string buildingName)
        {
            var item = await _repo.FindAll(x => x.Name == buildingName).AnyAsync();
            if (item)
            {
                return new OperationResult 
                { 
                    StatusCode = HttpStatusCode.OK, 
                    Message = "The building name already existed!", 
                    Success = false 
                };
            }
            operationResult = new OperationResult
            {
                StatusCode = HttpStatusCode.OK,
                Success = true,
                Data = item
            };
            return operationResult;
        }
        public async Task<OperationResult> CheckExistSiteNo(string siteNo)
        {
            throw new NotSupportedException();
        }
        public async Task<OperationResult> AddFormAsync(BuildingDto model)
        {

            throw new InvalidOperationException();
            
        }

        public async Task<object> DeleteUploadFile(int key)
        {
            
            throw new ArgumentException();
        }

        public async Task<object> CheckRoom()
        {
            var query = await _repo.FindAll().ToListAsync();

            return query;
        }
    }
}
