using BookingApp.DTO;
using BookingApp.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace BookingApp.Controllers
{
    public class Room2FacilityController : ApiControllerBase
    {
        private readonly IRoom2FacilityService _service;
        public Room2FacilityController(IRoom2FacilityService service)
        {
            _service = service;
        }
        [HttpGet]
        public async Task<ActionResult> GetAllAsync()
        {

            return Ok(await _service.GetAllAsync());
        }
        [HttpPost]
        public async Task<ActionResult> AddAsync([FromBody] Room2FacilityDto model)
        {
            return Ok(await _service.AddAsync(model));
        }
    }
}
