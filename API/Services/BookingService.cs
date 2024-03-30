using AutoMapper;
using AutoMapper.QueryableExtensions;
using BookingApp.Constants;
using BookingApp.Data;
using BookingApp.DTO;
using BookingApp.DTO.Filter;
using BookingApp.Helpers;
using BookingApp.Models;
using BookingApp.Services.Base;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using Renci.SshNet.Messages;
using Syncfusion.JavaScript;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace BookingApp.Services
{
    public interface IBookingService : IServiceBase<Booking, BookingDto>
    {
        Task<List<BookingDto>> ConflictChecking(BookingDto model);
        //Task<OperationResult> Disable(int id);
        Task<OperationResult> UpdateStatus(int id, int bookingStatus);
        Task<List<BookingDto>> SearchBooking(BookingFilter bookingFilter);
        DataTable GetBookingData(DateTime dateTime);
    }

    public class BookingService : ServiceBase<Booking, BookingDto>, IBookingService
    {
        private readonly IRepositoryBase<Booking> _repo;
        private readonly IRepositoryBase<User> _userRepository;
        private readonly IRepositoryBase<Floor> _floorRepository;
        private readonly IRepositoryBase<Room> _roomRepository;
        private readonly IRepositoryBase<Building> _buildingRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly MapperConfiguration _configMapper;
        private readonly IMailExtension _mailService;
        public BookingService(
                       IRepositoryBase<Booking> repo,
                       IRepositoryBase<User> user,
                       IRepositoryBase<Floor> floor,
                       IRepositoryBase<Room> room,
                       IRepositoryBase<Building> building,
                                  IUnitOfWork unitOfWork,
                                             IMapper mapper,
                                                        MapperConfiguration configMapper,
                                                        IMailExtension mailService)
                       
            : base(repo, unitOfWork, mapper, configMapper)
        {
            _repo = repo;
            _userRepository = user;
            _floorRepository = floor;
            _roomRepository = room;
            _buildingRepository = building;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _configMapper = configMapper;
            _mailService = mailService;
        }

        public override async Task<List<BookingDto>> GetAllAsync()
        {
            var query = _repo.FindAll(x => x.Status).ProjectTo<BookingDto>(_configMapper);
            var data = await query.ToListAsync();
            return data;
        }
        public async Task<List<BookingDto>> SearchBooking(BookingFilter bookingFilter)
        { //missing compare date
            var bookings = await _repo.FindAll(x => x.Status).ToListAsync();
            var user = await _userRepository.FindAll(x => x.Status).ToListAsync();
            var buildings = await _buildingRepository.FindAll(x=> x.Status).ToListAsync();
            var floors = await _floorRepository.FindAll(x => x.Status).ToListAsync();
            var rooms = await _roomRepository.FindAll(x => x.Status).ToListAsync();
            var query = from booking in bookings
                        select new BookingDto
                        {
                                ID = booking.ID,
                                UserGuid = booking.UserGuid,
                                UserName = booking.UserGuid == null ? user.Where(x => x.Guid == booking.UserGuid).FirstOrDefault().Name:"notFound", 
                                  CampusGuid = booking.CampusGuid,
                                  BuildingGuid = booking.BuildingGuid,
                                  BuildingName = booking.BuildingGuid==null ? buildings.Where(x => x.BuildingGuid == booking.BuildingGuid).FirstOrDefault().Name : "notFound",
                                  FloorGuid = booking.FloorGuid,
                                  FloorNumber = booking.FloorGuid==null ? floors.Where(x => x.Guid == booking.FloorGuid).FirstOrDefault().FloorNum:-1,
                                  RoomGuid = booking.RoomGuid,
                                  RoomNumber = booking.RoomGuid==null ? rooms.Where(x => x.Guid == booking.RoomGuid).FirstOrDefault().RoomNum:-1,
                                  BookingDate = booking.BookingDate,
                                  BookingTimeS = booking.BookingTimeS,
                                  BookingTimeE = booking.BookingTimeE,
                                  OrderDate = booking.OrderDate,
                                  StartDate = booking.StartDate,
                                  EndDate = booking.EndDate,
                                  Description = booking.Description,
                                  Comment = booking.Comment,
                                  BookingStatus = booking.BookingStatus,
                                  CreateDate = booking.CreateDate,
                                  CreateBy = booking.CreateBy,
                                  Status = booking.Status,
                                  BookingGuid = booking.BookingGuid
                              };

            if (!string.IsNullOrEmpty(bookingFilter.CampusGuid))
            {
                query = query.Where(x => x.CampusGuid == bookingFilter.CampusGuid);
            }
            if (!string.IsNullOrEmpty(bookingFilter.BuildingGuid))
            {
                query = query.Where(x => x.BuildingGuid == bookingFilter.BuildingGuid);
            }
            if (!string.IsNullOrEmpty(bookingFilter.FloorGuid))
            {
                query = query.Where(x => x.FloorGuid == bookingFilter.FloorGuid);
            }
            if (!string.IsNullOrEmpty(bookingFilter.RoomGuid))
            {
                query = query.Where(x => x.RoomGuid == bookingFilter.RoomGuid);
            }
            if (bookingFilter.StartDate != null)
            {
                query = query.Where(x => x.StartDate.Value >= bookingFilter.StartDate.Value);
            }
            if (bookingFilter.EndDate != null)
            {
                query = query.Where(x => x.EndDate.Value <= bookingFilter.EndDate.Value);
            }
            // add more filter here
            //var data = query;
            return query.ToList();
            

        }
        public override Task<BookingDto> GetByIDAsync(int id)
        {
            var booking = _repo.FindByID(id);
            var user = _userRepository.FindAll(x => x.Guid == booking.UserGuid).FirstOrDefault();
            var building = _buildingRepository.FindAll(x => x.BuildingGuid == booking.BuildingGuid).FirstOrDefault();
            var floor = _floorRepository.FindAll(x => x.Guid == booking.FloorGuid).FirstOrDefault();
            var room = _roomRepository.FindAll(x => x.Guid == booking.RoomGuid).FirstOrDefault();
            var bookingDto = new BookingDto { 
                ID = booking.ID,
                UserGuid = booking.UserGuid,
                UserName = user.Name,
                CampusGuid = booking.CampusGuid,
                BuildingGuid = booking.BuildingGuid,
                BuildingName = booking.BuildingGuid == null ? building.Name : "notFound",
                FloorGuid = booking.FloorGuid,
                FloorNumber = booking.FloorGuid == null ? floor.FloorNum : -1,
                RoomGuid = booking.RoomGuid,
                RoomNumber = booking.RoomGuid == null ? room.RoomNum : -1,
                BookingDate = booking.BookingDate,
                BookingTimeS = booking.BookingTimeS,
                BookingTimeE = booking.BookingTimeE,
                OrderDate = booking.OrderDate,
                StartDate = booking.StartDate,
                EndDate = booking.EndDate,
                Description = booking.Description,
                Comment = booking.Comment,
                BookingStatus = booking.BookingStatus,
                CreateDate = booking.CreateDate,
                CreateBy = booking.CreateBy,
                Status = booking.Status,
                BookingGuid = booking.BookingGuid
            };
            return Task.FromResult(bookingDto);
        }
        public override async Task<OperationResult> AddAsync(BookingDto model) 
        {
            try {
                var item = _mapper.Map<Booking>(model);
                var conflictBooking = ConflictChecking(model);
            if (conflictBooking.Result.Count > 0) { 
                return new OperationResult
                {
                    StatusCode = HttpStatusCode.OK, 
                    Message = MessageReponse.BookingErrorTimeConflict,
                    Success = false,
                    Data = conflictBooking.Result
                };
            }
            
            item.Status = true; //set status to true
            item.BookingStatus = BookingStatus.Pending; //set booking status to pending
            item.BookingGuid = Guid.NewGuid().ToString("N") + DateTime.Now.ToString("ssff").ToUpper();
            item.BookingTimeS = item.StartDate.ToString("HH:mm"); // convert start date to string
            item.BookingTimeE = item.EndDate.ToString("HH:mm"); // convert end date to string
            item.FloorGuid = _roomRepository.FindAll(x => x.Guid == item.RoomGuid).FirstOrDefault().FloorGuid; // get floor guid
            item.BuildingGuid = _floorRepository.FindAll(x => x.Guid == item.FloorGuid).FirstOrDefault().BuildingGuid; // get building guid
            //item.CampusGuid = _buildingRepository.FindAll(x => x.BuildingGuid == item.BuildingGuid).FirstOrDefault().CampusGuid; // get campus guid
            _repo.Add(item);
            var email = _userRepository.FindAll(x => x.Guid == item.UserGuid).FirstOrDefault().Email; // get user email
            var subject = Constants.Mailling.Subject.Create;
            var message = Constants.Mailling.Content.Create(item.BookingGuid, item.StartDate, item.EndDate, item.RoomGuid, item.CampusGuid);
            await _mailService.SendEmailAsync(email, subject, message);
            await _unitOfWork.SaveChangeAsync();
            operationResult = new OperationResult
            {
                StatusCode = HttpStatusCode.OK,
                Message = MessageReponse.BookingSuccess,
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

        public async Task<OperationResult> UpdateStatus(int id, int bookingStatus) // function for admin to change booking status
        {
            var item = _repo.FindByID(id);
            item.BookingStatus = bookingStatus;
            var email = _userRepository.FindAll(x => x.Guid == item.UserGuid).FirstOrDefault().Email; // get user email
            var subject = Constants.Mailling.Subject.Update;
            var message = Constants.Mailling.Content.Update(item.BookingGuid, bookingStatus);
            await _mailService.SendEmailAsync(email, subject, message);
            _repo.Update(item);
            try
            {
                await _unitOfWork.SaveChangeAsync();
                operationResult = new OperationResult
                {
                    StatusCode = HttpStatusCode.OK,
                    Message = MessageReponse.UpdateSuccess,
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
        public override async Task<OperationResult> UpdateAsync(BookingDto model)
        {
            var item = _mapper.Map<Booking>(model);
            var conflictBooking = ConflictChecking(model);
            if (conflictBooking.Result.Count > 0)
            {
                return new OperationResult
                {
                    StatusCode = HttpStatusCode.OK, // ?? should it be ok ?
                    Message = MessageReponse.BookingErrorTimeConflict,
                    Success = false,
                    Data = conflictBooking.Result
                };
            }
            item.BookingStatus = BookingStatus.Pending; //set booking status to pending
            item.BookingTimeS = item.StartDate.ToString("HH:mm"); // convert start date to string
            item.BookingTimeE = item.EndDate.ToString("HH:mm"); // convert end date to string
            _repo.Update(item);

            try
            {
                await _unitOfWork.SaveChangeAsync();
                operationResult = new OperationResult
                {
                    StatusCode = HttpStatusCode.OK,
                    Message = MessageReponse.UpdateSuccess,
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
        public async Task<List<BookingDto>> ConflictChecking(BookingDto model) //Logic checking for Room Booking
        {
            var query = _repo.FindAll().ProjectTo<BookingDto>(_configMapper);
            var item = _mapper.Map<Booking>(model);
            var conflictBooking = query.Where(x =>
                x.Status == true && // only check active booking (x.Status could be changed to x.bookingStatus)
                x.CampusGuid == item.CampusGuid && // check campus conflict
                x.BuildingGuid == item.BuildingGuid && // check building conflict
                x.FloorGuid == item.FloorGuid && // check floor conflict
                x.RoomGuid == item.RoomGuid && // check room conflict
                item.StartDate < item.EndDate &&  // check if start date is before end date
                (x.StartDate <= item.StartDate && item.StartDate < x.EndDate || // check start date conflict, if the start date of the new booking is between the start and end date of the existing booking
                x.StartDate < item.EndDate && item.EndDate <= x.EndDate) 
                ) // check end date conflict, if the end date of the new booking is between the start and end date of the existing booking
                    .ToListAsync();
            return await conflictBooking;
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

        public DataTable GetBookingData(DateTime dateTime)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("BookingID", typeof(int));
            dt.Columns.Add("BookingDate", typeof(DateTime));
            dt.Columns.Add("BookingTimeS", typeof(string));
            dt.Columns.Add("BookingTimeE", typeof(string));
            dt.Columns.Add("RoomGuid", typeof(string));
            dt.Columns.Add("FloorGuid", typeof(string));
            dt.Columns.Add("BuildingGuid", typeof(string));
            dt.Columns.Add("CampusGuid", typeof(string));
            dt.Columns.Add("UserGuid", typeof(string));
            dt.Columns.Add("BookingStatus", typeof(int));
            //var _list = _repo.FindAll(x => x.StartDate.Day == dateTime.Day).ToList();
            var _list = _repo.FindAll().ToList(); // get all booking to test
            foreach (var item in _list)
            {
                dt.Rows.Add(item.ID, item.BookingDate, item.BookingTimeS, item.BookingTimeE, item.RoomGuid, item.FloorGuid, item.BuildingGuid, item.CampusGuid, item.UserGuid, item.BookingStatus);
            }
            return dt;
        }

    }
}
