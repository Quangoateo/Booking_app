using Microsoft.Extensions.Configuration;
using Quartz;
using Quartz.Impl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookingApp.Helpers.ScheduleQuartz
{
    public class SchedulerBase<TClass> where TClass : IJob
    {
        IScheduler _scheduler;
        IJobDetail _job;
        ITrigger _trigger;
        private readonly IConfiguration _configuration;

      
        public SchedulerBase(IConfiguration Configuration)
        {
            _configuration = Configuration;
        }

        public async Task Start(int hour, int minute)
        {
            _scheduler = await StdSchedulerFactory.GetDefaultScheduler();
            await _scheduler.Start();
            _job = JobBuilder.Create<TClass>().Build();

            _trigger = TriggerBuilder.Create()
                .WithDailyTimeIntervalSchedule
                  (s =>
                     s.WithIntervalInHours(24)
                    .OnEveryDay()
                    .StartingDailyAt(TimeOfDay.HourAndMinuteOfDay(hour, minute))
                  )

                .Build();
            await _scheduler.ScheduleJob(_job, _trigger);
        }
        public async Task StartAutoSendMail(int repeatMinute, TimeSpan startHourAt, TimeSpan endHourAt)
        {
            var appsettings = _configuration.GetSection("MailSettings").Get<Mailsettings>();
            IDictionary<string, object> map = new Dictionary<string, object>();
            map.Add("API_URL", appsettings.API_URL);
            map.Add("Server", appsettings.Server);
            map.Add("UseDefaultCredentials", appsettings.UseDefaultCredentials);
            map.Add("Port", appsettings.Port);
            map.Add("EnableSsl", appsettings.EnableSsl);
            map.Add("UserName", appsettings.UserName);
            map.Add("Password", appsettings.Password);
            map.Add("FromEmail", appsettings.FromEmail);
            map.Add("FromName", appsettings.FromName);
            _scheduler = await StdSchedulerFactory.GetDefaultScheduler();

            await _scheduler.Start();

            _job = JobBuilder.Create<TClass>()
                .SetJobData(new JobDataMap(map))
                .Build();
            var st = DateTime.Now.Date.Add(startHourAt);
            var end = DateTimeOffset.Now.Date.Add(endHourAt);
            _trigger = TriggerBuilder.Create()
                        .StartAt(st)
                        .WithSchedule(SimpleScheduleBuilder.RepeatMinutelyForever(int.Parse(appsettings.TimeAsync)))
                        .EndAt(end)
                        .Build();
            await _scheduler.ScheduleJob(_job, _trigger);
        }

        public async Task StartAsyncOracle(int repeatMinute, TimeSpan startHourAt, TimeSpan endHourAt)
        {
            var appsettings = _configuration.GetSection("OracleSettings").Get<OracleSettings>();
            var _connect = _configuration.GetSection("ConnectionStrings").GetSection("DefaultConnection").Value;
            var oracle_connect = _configuration.GetSection("ConnectionStrings").GetSection("OracleConnection").Value;
            IDictionary<string, object> map = new Dictionary<string, object>();
            map.Add("API_URL", appsettings.API_URL); 
            map.Add("SQL_CONNECT", _connect);
            map.Add("ORACLE_CONNECT", oracle_connect);

            _scheduler = await StdSchedulerFactory.GetDefaultScheduler();
            await _scheduler.Start();

            _job = JobBuilder.Create<TClass>()
                .SetJobData(new JobDataMap(map))
                .Build();
            var st = DateTime.Now.Date.Add(startHourAt);
            var end = DateTimeOffset.Now.Date.Add(endHourAt);
            _trigger = TriggerBuilder.Create()
                        .StartAt(st)
                        .WithSchedule(SimpleScheduleBuilder.RepeatMinutelyForever(int.Parse(appsettings.TimeAsync)))
                        .EndAt(end)
                        .Build();
            await _scheduler.ScheduleJob(_job, _trigger);
        }
        public async Task Start(SimpleScheduleBuilder repeatMinute, TimeSpan startHourAt, TimeSpan endHourAt)
        {
            var ct = DateTime.Now.ToLocalTime();
            _scheduler = await StdSchedulerFactory.GetDefaultScheduler();
            await _scheduler.Start();
            _job = JobBuilder.Create<TClass>().Build();
            var st = DateTime.Now.Date.Add(startHourAt);
            var end = DateTimeOffset.Now.Date.Add(endHourAt);
            _trigger = TriggerBuilder.Create()
                        .StartAt(st)
                        .WithSchedule(repeatMinute)
                        .EndAt(end)
                        .Build();
            await _scheduler.ScheduleJob(_job, _trigger);
        }
        public async Task Start(IntervalUnit intervalUnit, DayOfWeek dayofWeek, int hour, int minute)
        {

            _scheduler = await StdSchedulerFactory.GetDefaultScheduler();
            await _scheduler.Start();
            _job = JobBuilder.Create<TClass>().Build();

            _trigger = TriggerBuilder.Create()
                .WithDailyTimeIntervalSchedule
                  (s =>
                    s.WithInterval(1, intervalUnit)
                    .OnDaysOfTheWeek(dayofWeek)
                    .StartingDailyAt(TimeOfDay.HourAndMinuteOfDay(hour, minute))
                  )
                .Build();
            await _scheduler.ScheduleJob(_job, _trigger);
        }
        public async Task Start(IntervalUnit intervalUnit, int hour, int minute)
        {

            _scheduler = await StdSchedulerFactory.GetDefaultScheduler();
            await _scheduler.Start();
            _job = JobBuilder.Create<TClass>().Build();

            _trigger = TriggerBuilder.Create()
                .WithDailyTimeIntervalSchedule
                  (s =>
                    s.WithInterval(1, intervalUnit)
                    .StartingDailyAt(TimeOfDay.HourAndMinuteOfDay(hour, minute))
                  )
                .Build();
            await _scheduler.ScheduleJob(_job, _trigger);
        }
        public async Task Start(int interval = 1, IntervalUnit intervalUnit = IntervalUnit.Hour, int hour = 6, int minute = 0)
        {
            //var appsettings = _configuration.GetSection("Appsettings").Get<Appsettings>();
            //IDictionary<string, object> map = new Dictionary<string, object>();
            //map.Add("Signalr_URL", appsettings.Signalr_URL);
            _scheduler = await StdSchedulerFactory.GetDefaultScheduler();
            await _scheduler.Start();

            _job = JobBuilder.Create<TClass>()
                //.SetJobData(new JobDataMap(map))
                .Build();

            _trigger = TriggerBuilder.Create()

                .WithDailyTimeIntervalSchedule
                  (s =>
                    s.WithInterval(interval, intervalUnit)
                    .StartingDailyAt(TimeOfDay.HourAndMinuteOfDay(hour, minute))
                  )
                .Build();
            await _scheduler.ScheduleJob(_job, _trigger);
        }
        public async Task<bool> checkScheduleStart()
        {
            _scheduler = await StdSchedulerFactory.GetDefaultScheduler();
            return _scheduler.IsStarted;


        }

        public async Task Stop()
        {
            if (_scheduler.IsStarted)
            {
                await _scheduler.Shutdown();
            }
        }
    }
}
