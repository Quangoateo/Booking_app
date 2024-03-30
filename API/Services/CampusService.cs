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
    public interface ICampusService : IServiceBase<Campus, CampusDto>
    {

    }
    public class CampusService : ServiceBase<Campus, CampusDto>, ICampusService
    {
        private readonly IRepositoryBase<Campus> _repo;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly MapperConfiguration _configMapper;
        private readonly IWebHostEnvironment _currentEnvironment;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public CampusService(
            IRepositoryBase<Campus> repo,
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

        public override async Task<OperationResult> AddAsync(CampusDto model)
        {
            try
            {
                var item = _mapper.Map<Campus>(model);
                item.Status = true;
                item.Guid = Guid.NewGuid().ToString("N") + DateTime.Now.ToString("ssff").ToUpper();
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
        public override async Task<List<CampusDto>> GetAllAsync()
        {
            var query = _repo.FindAll(x => x.Status).ProjectTo<CampusDto>(_configMapper);

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


        
    }
}
