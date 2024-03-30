using BookingApp.Data;
using BookingApp.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace BookingApp.Helpers
{
    public class LoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<LoggingMiddleware> _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public LoggingMiddleware(RequestDelegate next,
            ILogger<LoggingMiddleware> logger,
            IHttpContextAccessor httpContextAccessor)
        {
            _next = next;
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task Invoke(HttpContext context, BookingAppContext bookingAppContext)
        {
            await _next(context);
            var accessToken = _httpContextAccessor.HttpContext.Request.Headers["Authorization"];
            bookingAppContext.Log.Add(new Log
            {
                TimeStamp = System.DateTime.Now,
                UserID = JWTExtensions.GetDecodeTokenByID(accessToken),
                EventType = context.GetRouteData().Values["action"]?.ToString(),
                EventName = context.GetRouteData().Values["controller"]?.ToString(),
                Status = context.Response.StatusCode.ToString()
            });
            await bookingAppContext.SaveChangesAsync();

        }
    }
}
