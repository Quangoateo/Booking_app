using AutoMapper;
using AutoMapper.QueryableExtensions;
using BookingApp.Constants;
using BookingApp.Data;
using BookingApp.DTO;
using BookingApp.DTO.Filter;
using BookingApp.Helpers;
using BookingApp.Models;
using BookingApp.Services.Base;
using Microsoft.EntityFrameworkCore;
using Quartz.Util;
using Syncfusion.JavaScript;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace BookingApp.Services
{
    public interface IRoomService : IServiceBase<Room, RoomDto>
    {
        Task<object> LoadData(DataManager data, string farmGuid);
        Task<object> GetAudit(object id);
        Task<OperationResult> AddFormAsync(RoomDto model);
        Task<object> GetSitesByAccount();
        Task<object> CheckRoom();
        Task<List<RoomDto>> SearchRoom(RoomFilter roomFilter);
    }

    public class RoomService : ServiceBase<Room, RoomDto>, IRoomService
    {
        private readonly IRepositoryBase<Room> _repo;
        private readonly IRepositoryBase<Floor> _repoFloor;
        private readonly IRepositoryBase<Building> _repoBuilding;
        private readonly IRepositoryBase<Room2Facility> _repoRoom2Facility;
        private readonly IRepositoryBase<Facility> _repoFacility;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly MapperConfiguration _configMapper;

        public RoomService(
            IRepositoryBase<Room> repo,
            IRepositoryBase<Floor> repoFloor,
            IRepositoryBase<Building> repoBuilding,
            IUnitOfWork unitOfWork,
            IMapper mapper,
            MapperConfiguration configMapper, IRepositoryBase<Room2Facility> repoRoom2Facility, IRepositoryBase<Facility> repoFacility)
            : base(repo, unitOfWork, mapper, configMapper)
        {
            _repo = repo;
            _repoFloor = repoFloor;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _configMapper = configMapper;
            _repoRoom2Facility = repoRoom2Facility;
            _repoFacility = repoFacility;
            _repoBuilding = repoBuilding;
        }

        public override async Task<List<RoomDto>> GetAllAsync()
        {
            var query = _repo.FindAll(x => x.Status).ProjectTo<RoomDto>(_configMapper);
            var data = await query.ToListAsync();
            return data;
        }
        public override Task<RoomDto> GetByIDAsync(int id)
        {
            var room = _repo.FindByID(id);
            var floor = _repoFloor.FindAll(x => x.Guid == room.FloorGuid && x.Status).FirstOrDefault();
            var building = _repoBuilding.FindAll(x => x.BuildingGuid == floor.BuildingGuid && x.Status).FirstOrDefault();
            var facilityList = _repoRoom2Facility.FindAll(x => x.RoomGuid == room.Guid && x.Status).ToList();
            var facility = _repoFacility.FindAll(x => x.Status).ToList();
            var joinFacility = facilityList
                .Join(facility, f => f.FacilityGuid, b => b.Guid, (f, b) => new { f, b });
            var roomDto = new RoomDto
            {
                ID = room.ID,
                FloorGuid = room.FloorGuid,
                FloorNum = floor.FloorNum,
                BuildingGuid = floor.BuildingGuid,
                BuildingName = building.Name,
                RoomNum = room.RoomNum,
                RoomTypeID = room.RoomTypeID,
                Description = room.Description,
                Capacity = room.Capacity,
                Facilities = joinFacility.Where(x => x.f.RoomGuid == room.Guid).Select(x => new FacilityDto
                {
                    ID = x.b.ID,
                    Name = x.b.Name,
                    Type = x.b.Type,
                    Brand = x.b.Brand,
                    Number = x.f.Number,
                    CreateDate = x.b.CreateDate,
                    CreateBy = x.b.CreateBy,
                    Status = x.b.Status,
                    Guid = x.b.Guid
                }).ToList(),
                CreateDate = room.CreateDate,
                CreateBy = room.CreateBy,
                Status = room.Status,
                Guid = room.Guid
            };
            return Task.FromResult(roomDto);
        }

        public async Task<List<RoomDto>> SearchRoom(RoomFilter roomFilter) // IQueryable
        {
            var floors = await _repoFloor.FindAll(x => x.Status).ToListAsync();
            var buildings = await _repoBuilding.FindAll(x => x.Status).ToListAsync();
            var floor2Building = floors
                .Join(buildings, f => f.BuildingGuid, b => b.BuildingGuid, (f, b) => new { f, b }).
                ToList();
            var Rooms = await _repo.FindAll(x => x.Status).ToListAsync();
            var facilityList = await _repoRoom2Facility.FindAll(x => x.Status).ToListAsync();
            var facility = await _repoFacility.FindAll(x => x.Status).ToListAsync();
            var joinFacility = facilityList
                .Join(facility, f => f.FacilityGuid, b => b.Guid, (f, b) => new { f, b });
            var query = Rooms.Join(floor2Building, r => r.FloorGuid, fb => fb.f.Guid, (r, fb) => new { r, fb }).
                Select(x => new RoomDto
                {
                    ID = x.r.ID,
                    FloorGuid = x.r.FloorGuid,
                    FloorNum = x.fb.f.FloorNum,
                    BuildingGuid = x.fb.f.BuildingGuid,
                    BuildingName = x.fb.b.Name,
                    RoomNum = x.r.RoomNum,
                    RoomTypeID = x.r.RoomTypeID,
                    Description = x.r.Description,
                    Capacity = x.r.Capacity,
                    Facilities = joinFacility.Where(f => f.f.RoomGuid == x.r.Guid).Select(x => new FacilityDto
                    {
                        ID = x.b.ID,
                        Name = x.b.Name,
                        Type = x.b.Type,
                        Brand = x.b.Brand,
                        Number = x.f.Number,
                        CreateDate = x.b.CreateDate,
                        CreateBy = x.b.CreateBy,
                        Status = x.b.Status,
                        Guid = x.b.Guid
                    }).ToList(),
                    CreateDate = x.r.CreateDate,
                    CreateBy = x.r.CreateBy,
                    Status = x.r.Status,
                    Guid = x.r.Guid
                });
            if (!string.IsNullOrWhiteSpace(roomFilter.BuildingGuid))
            {
                query = query.Where(x => x.BuildingGuid == roomFilter.BuildingGuid);
            }
            if (!string.IsNullOrWhiteSpace(roomFilter.FloorGuid))
            {
                query = query.Where(x => x.FloorGuid == roomFilter.FloorGuid);
            }
            if (roomFilter.RoomTypeID != null)
            {
                query = query.Where(x => x.RoomTypeID == roomFilter.RoomTypeID);
            }
            if (!string.IsNullOrWhiteSpace(roomFilter.RoomGuid))
            {
                query = query.Where(x => x.Guid == roomFilter.RoomGuid);
            }
            return query.ToList();
            
        }

        public override async Task<OperationResult> AddAsync(Room model)
        {
            try
            {
                var item = _mapper.Map<Room>(model);
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

        public async Task<object> GetSitesByAccount()
        {
            throw new System.NotImplementedException();
        } // ?? not implemented

        public async Task<object> CheckRoom()
        {
            throw new System.NotImplementedException();
        } // ?? not implemented

        public async Task<OperationResult> AddFormAsync(RoomDto model)
        {
            throw new System.NotImplementedException();
        } // ?? not implemented

        public async Task<object> GetAudit(object id)
        {
            throw new System.NotImplementedException();
        } // ?? not implemented

        public async Task<object> LoadData(DataManager data, string farmGuid)
        {
            throw new System.NotImplementedException();
        } // ?? not implemented

        public async Task<object> DeleteUploadFile(int key)
        {
            var item = _repo.FindByID(key);
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

        public override async Task<OperationResult> DeleteAsync(int id)
        {
            var item = _repo.FindByID(id);
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