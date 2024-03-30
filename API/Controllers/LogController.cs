using BookingApp.DTO;
using BookingApp.Helpers;
using BookingApp.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace BookingApp.Controllers
{
    public class LogController : ApiControllerBase
    {

        private readonly ILogService _service;
        public LogController(ILogService service)
        {
            _service = service;
        }
        [HttpGet]
        public async Task<ActionResult> GetAllAsync()
        {
            return Ok(await _service.GetAllAsync());
        }
        [HttpGet]
        public async Task<ActionResult> GetWithPaginationsAsync([FromQuery]PaginationParams param) { 
            return Ok(await _service.GetWithPaginationsAsync(param));
        }

    }
}
