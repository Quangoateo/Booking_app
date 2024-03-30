using AutoMapper;
using AutoMapper.QueryableExtensions;
using BookingApp.Data;
//using BookingApp.Dto;
using BookingApp.DTO;
using BookingApp.Models;
using BookingApp.Services.Base;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using Syncfusion.JavaScript;
using Syncfusion.Linq;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookingApp.Services
{

    public interface ILogService : IServiceBase<Log, LogDto>
    {


    }
    public class LogService : ServiceBase<Log, LogDto>, ILogService
    {
        private readonly IRepositoryBase<Log> _repo;
        private readonly IRepositoryBase<User> _userRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly MapperConfiguration _configMapper;
        private readonly IWebHostEnvironment _currentEnvironment;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public LogService(
            IRepositoryBase<Log> repo,
            IRepositoryBase<User> userRepository,
            IUnitOfWork unitOfWork,
            IMapper mapper,
            IWebHostEnvironment currentEnvironment,
            IHttpContextAccessor httpContextAccessor,
            MapperConfiguration configMapper
            )
            : base(repo, unitOfWork, mapper, configMapper)
        {
            _repo = repo;
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _configMapper = configMapper;
            _currentEnvironment = currentEnvironment;
            _httpContextAccessor = httpContextAccessor;
        }
        //public override async Task<List<LogDto>> GetAllAsync()
        //{
        //    var logs = await _repo.FindAll().ToListAsync();

        //    var userIds = logs.Select(log => log.UserID).Distinct();
        //    var users = await _userRepository.FindAll(u => userIds.Contains(u.ID))
        //                                     .ToDictionaryAsync(u => u.ID, u => u.LdapName);


        //    // Map Logs to LogDtos and set UserNames
        //    var logDtos = _mapper.Map<List<LogDto>>(logs); // Map from Logs to list of LogDto

        //    foreach (var dto in logDtos)
        //    {
        //        // Find the Log that this Dto is mapped from
        //        var matchingLog = logs.FirstOrDefault(log => log.Id == dto.Id);

        //        if (matchingLog != null && users.TryGetValue(matchingLog.UserID, out string userName))
        //        {
        //            dto.UserName = userName;
        //        }
        //        else
        //        {
        //            // Handle the case where a user is not found or the Log doesn't have a matching Dto (if desired)
        //        }
        //    }

        //    return logDtos;
        //}

        public override async Task<List<LogDto>> GetAllAsync()
        {
            var logs =  _repo.FindAll();
            var users =  _userRepository.FindAll();
            //var query = from log in logs
            //            join user in users on log.UserID equals user.ID
            //            into userJoin // left join
            //            from user in userJoin.DefaultIfEmpty()
            //            select new LogDto
            //            {
            //                Id = log.Id,
            //                TimeStamp = log.TimeStamp,
            //                UserName = user.LdapName,
            //                EventType = log.EventType,
            //                EventName = log.EventName,
            //                Status = log.Status
            //            };
            var query = from log in logs
                        join user in users on log.UserID equals user.ID into userJoin // Left join
                        from user in userJoin.DefaultIfEmpty() // Handle missing user cases
                        select new LogDto
                        {
                            Id = log.Id,
                            TimeStamp = log.TimeStamp,
                            UserName = log.UserID == 0 ? "guest" : user.LdapName,
                            EventType = log.EventType,
                            EventName = log.EventName,
                            Status = log.Status
                        };
            return await query.ToListAsync();
        }
    }

}
