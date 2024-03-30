using Microsoft.AspNetCore.Mvc;
using BookingApp.DTO;
using BookingApp.Helpers;
using BookingApp.Services;
using Syncfusion.JavaScript;
using System.Threading.Tasks;
using BookingApp.DTO.Filter;

namespace BookingApp.Controllers
{
    public class RoomController : ApiControllerBase
    {
        private readonly IRoomService _service;

        public RoomController(IRoomService service)
        {
            _service = service;
        }

        //[HttpGet]
        //public async Task<ActionResult> GetSitesByAccount()
        //{
        //    return Ok(await _service.GetSitesByAccount());
        //}

        [HttpGet]
        public async Task<ActionResult> GetAllAsync() 
        {
            return Ok(await _service.GetAllAsync());
        }
        [HttpGet]
        public async Task<ActionResult> SearchRoom([FromQuery] RoomFilter roomFilter)
        {
            return Ok(await _service.SearchRoom(roomFilter));
        }
        [HttpGet]
        public async Task<ActionResult> GetByIDAsync(int id)
        {
            return Ok(await _service.GetByIDAsync(id));
        }
        [HttpPost]
        public async Task<ActionResult> AddAsync([FromBody] RoomDto model)
        {
            return StatusCodeResult(await _service.AddAsync(model));
        }
        [HttpPut]
        public async Task<ActionResult> UpdateAsync([FromBody] RoomDto model)
        {
            return StatusCodeResult(await _service.UpdateAsync(model));
        }
        [HttpDelete]
        public async Task<ActionResult> DeleteAsync(int id)
        {
            return StatusCodeResult(await _service.DeleteAsync(id));
        }
    }
}
