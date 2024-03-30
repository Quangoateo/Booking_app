using BookingApp.Helpers;
using BookingApp.Helpers.ScheduleQuartz;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Quartz;
using System;
using System.Linq;

namespace BookingApp.Installer
{
    public static class InstallerExtensions
    {
        public static void InstallServicesInAssembly(this IServiceCollection services, IConfiguration configuration)
        {
            var installers = typeof(Startup).Assembly.ExportedTypes.Where(x =>
           typeof(IInstaller).IsAssignableFrom(x) && !x.IsInterface && !x.IsAbstract).Select(Activator.CreateInstance).Cast<IInstaller>().ToList();
            installers.ForEach(installer => installer.InstallServices(services, configuration));
        }

        public static IServiceCollection AddShedulerExtention(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddQuartz(async q =>
            {
                //q.UseMicrosoftDependencyInjectionJobFactory();
                q.SchedulerId = "BookingApp";

                // Thuc thi luc 6:00, lap lai 1 tieng 1 lan
                //var option = new ReloadTodoJob();
                //await new SchedulerBase<ReloadTodoJob>(configuration).Start(1, IntervalUnit.Hour, 6, 00);

                // Thuc thi luc 6:00 đên 21 gio la ngung lap lai 30 phut 1 lan

                var startAt = TimeSpan.FromHours(6);
                var endAt = TimeSpan.FromHours(21);
                var repeatMins = 1;
                await new SchedulerBase<SendMailJob>(configuration).StartAutoSendMail(repeatMins, startAt, endAt);
                //await new SchedulerBase<AsyncOracleJob>(configuration).StartAsyncOracle(repeatMins, startAt, endAt);
            });
            services.AddQuartzHostedService(q => q.WaitForJobsToComplete = true);
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
