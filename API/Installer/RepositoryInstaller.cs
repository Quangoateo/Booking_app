using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using BookingApp.Data;
using System;

namespace BookingApp.Installer
{
    public class RepositoryInstaller : IInstaller
    {
     

        public void InstallServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped(typeof(IRepositoryBase<>), typeof(RepositoryBase<>));
        }
    }
}
