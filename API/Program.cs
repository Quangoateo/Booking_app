using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using System.Linq;
using System.Net;
using Quartz;
using System;
using BookingApp.Helpers.ScheduleQuartz;


namespace BookingApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }
        //public static IHostBuilder CreateHostBuilder(string[] args) =>
        //    Host.CreateDefaultBuilder(args)
        //        .ConfigureWebHostDefaults(webBuilder =>
        //        {
        //            webBuilder.UseStartup<Startup>();
        //        })
        //   .ConfigureWebHost(config =>
        //   {
        //       config.UseUrls("http://*:5003/");
        //   })
        //   .UseWindowsService();
        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
#if DEBUG
                    webBuilder.UseKestrel(opts =>
            {
                string HostName = Dns.GetHostName();
                IPAddress[] ipaddress = Dns.GetHostAddresses(HostName);
                foreach (IPAddress ip4 in ipaddress.Where(ip => ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork))
                {
                    opts.Listen(ip4, port: 58);
                }
                opts.ListenAnyIP(port: 58);

            });
#endif

                });
            //.ConfigureWebHost(config =>
            //{
            //    config.UseUrls("http://*:5002/");
            //}).UseWindowsService();
        //.ConfigureServices((hostContext, services) =>
        //{
        //    // see Quartz.Extensions.DependencyInjection documentation about how to configure different configuration aspects
        //    services.AddQuartz(async q =>
        //    {
        //        // your configuration here
        //        //q.UseMicrosoftDependencyInjectionJobFactory();
        //        q.SchedulerId = "BookingApp";

        //        // Thuc thi luc 6:00, lap lai 1 tieng 1 lan
        //        //var option = new ReloadTodoJob();
        //        //await new SchedulerBase<ReloadTodoJob>(configuration).Start(1, IntervalUnit.Hour, 6, 00);

        //        // Thuc thi luc 6:00 đên 21 gio la ngung lap lai 30 phut 1 lan

        //        var startAt = TimeSpan.FromHours(6);
        //        var endAt = TimeSpan.FromHours(21);
        //        var repeatMins = 1;
        //        //await new SchedulerBase<SendMailJob>(configuration).Start(repeatMins, startAt, endAt);
        //        await new SchedulerBase<AsyncOracleJob>(hostContext.Configuration).StartAsyncOracle(repeatMins, startAt, endAt);
        //    });

        //    // Quartz.Extensions.Hosting hosting
        //    services.AddQuartzHostedService(options =>
        //    {
        //        // when shutting down we want jobs to complete gracefully
        //        options.WaitForJobsToComplete = true;
        //    });
        //});


    }
}
