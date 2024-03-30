using Microsoft.AspNetCore.Mvc;
using BookingApp.DTO;
using BookingApp.Helpers;
using BookingApp.Services;
using Syncfusion.JavaScript;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace BookingApp.Controllers
{
    public class FloorController : ApiControllerBase
    {
        private readonly IFloorService _service;

        public FloorController(IFloorService service)
        {
            _service = service;
        }
        [HttpGet]
        public async Task<ActionResult> GetAllAsync()
        {
            return Ok(await _service.GetAllAsync());
        }



        [HttpDelete]
        public async Task<ActionResult> DeleteAsync(int id)
        {
            return StatusCodeResult(await _service.DeleteAsync(id));
        }

        [HttpGet]
        public async Task<ActionResult> GetByIDAsync(int id)
        {
            return Ok(await _service.GetByIDAsync(id));
        }

        [HttpGet]
        public async Task<ActionResult> GetWithPaginationsAsync([FromQuery]PaginationParams paramater)
        {
            return Ok(await _service.GetWithPaginationsAsync(paramater));
        }
        [HttpPost]
        public async Task<ActionResult> AddAsync(FloorDto model)
        {
            return StatusCodeResult(await _service.AddAsync(model));
        }
        [HttpPost]
        public async Task<ActionResult> AddRangeAsync(List<FloorDto> model)
        {
            return StatusCodeResult(await _service.AddRangeAsync(model));
        }
        [HttpPut]
        public async Task<ActionResult> UpdateAsync(FloorDto model)
        {
            return StatusCodeResult(await _service.UpdateAsync(model));
        }
    }
}
