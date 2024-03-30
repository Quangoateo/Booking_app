using AutoMapper;
using AutoMapper.QueryableExtensions;
using BookingApp.Constants;
using BookingApp.Data;
using BookingApp.DTO;
using BookingApp.Helpers;
using BookingApp.Models;
using BookingApp.Services.Base;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Syncfusion.JavaScript.DataVisualization.Builders;
using System;
using BookingApp.DTO.Filter;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Renci.SshNet.Security.Cryptography;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace BookingApp.Services
{
    public interface IFloorService : IServiceBase<Floor, FloorDto>
    {
        Task<List<FloorDto>> SearchFloor(FloorFilter floorFilter);
    }
    public class FloorService : ServiceBase<Floor, FloorDto>, IFloorService
    {
        private readonly IRepositoryBase<Floor> _repo;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly MapperConfiguration _configMapper;
        private readonly IWebHostEnvironment _currentEnvironment;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public FloorService(
            IRepositoryBase<Floor> repo,
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
        public async Task<List<FloorDto>> SearchFloor (FloorFilter floorFilter)
        {
            var data = await _repo.FindAll(x => x.Status).AsQueryable().ProjectTo<FloorDto>(_configMapper).ToListAsync();
            if (floorFilter.FloorNum != null)
            {
                data = data.Where(x => x.FloorNum == floorFilter.FloorNum).ToList();
            }
            if (floorFilter.BuildingGuid != null)
            {
                data = data.Where(x => x.BuildingGuid == floorFilter.BuildingGuid).ToList();
            }
            if (floorFilter.CampusGuid != null)
            {
                data = data.Where(x => x.BuildingGuid == floorFilter.CampusGuid).ToList();
            }
            return data;
        }
    }
}