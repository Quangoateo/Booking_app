using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using BookingApp.Helpers;
using BookingApp.Services;
using Quartz;
using System;

namespace BookingApp.Installer
{
    public class ScheduleInstaller
    {
        public static IServiceCollection InstallSchedule(IServiceCollection services, IConfiguration configuration)
        {
            services.AddQuartz(async q =>
            {
                q.SchedulerId = "BookingApp";

                // Thuc thi luc 6:00, lap lai 1 tieng 1 lan
                //var option = new ReloadTodoJob();
                //await new SchedulerBase<ReloadTodoJob>(configuration).Start(1, IntervalUnit.Hour, 6, 00);

                // Thuc thi luc 6:00 đên 21 gio la ngung lap lai 30 phut 1 lan
                var startAt = TimeSpan.FromHours(6);
                var endAt = TimeSpan.FromHours(21);
                var repeatMins = 30;
                //await new SchedulerBase<ReloadDispatchJob>(configuration).Start(repeatMins, startAt, endAt);
            });

            // ASP.NET Core hosting
            services.AddQuartzServer(options =>
            {
                // when shutting down we want jobs to complete gracefully
                options.WaitForJobsToComplete = true;
            });
            return services;
        }

      
    }
}
