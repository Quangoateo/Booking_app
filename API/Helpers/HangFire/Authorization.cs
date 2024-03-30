using Hangfire.Dashboard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookingApp.Helpers.HangFire
{
    public class Authorization : IDashboardAuthorizationFilter
    {
        public bool Authorize(DashboardContext context)
        {
            return true;
            //var httpContext = context.GetHttpContext();

            //// Allow all authenticated users to see the Dashboard (potentially dangerous).
            //return httpContext.User.Identity?.IsAuthenticated ?? false;
        }
    }
}
