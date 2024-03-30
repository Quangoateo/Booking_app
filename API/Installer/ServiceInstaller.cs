using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using BookingApp.Helpers;
using BookingApp.Services;
using Quartz;
using System;
using BookingApp.Helpers.ScheduleQuartz;

namespace BookingApp.Installer
{
    public class ServiceInstaller : IInstaller
    {

        public void InstallServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IBuildingService, BuildingService>();
            services.AddScoped<ICampusService, CampusService>();
            services.AddScoped<IFacilityService, FacilityService>();
            services.AddScoped<IRoom2FacilityService, Room2FacilityService>();
            services.AddScoped<ILdapService, LdapService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IRoomService, RoomService>();
            services.AddScoped<IFloorService, FloorService>();
            services.AddScoped<IBookingService, BookingService>();
            services.AddScoped<IMailExtension, MailExtension>();
            services.AddScoped<ILogService, LogService>();
            services.AddScoped<IRoleService, RoleService>();
            services.AddScoped<IRolePermissionService, RolePermissionService>();

        }
    }
}
