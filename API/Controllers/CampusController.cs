using Microsoft.AspNetCore.Mvc;
using BookingApp.DTO;
using BookingApp.Helpers;
using BookingApp.Services;
using Syncfusion.JavaScript;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace BookingApp.Controllers
{
    public class CampusController : ApiControllerBase
    {
        private readonly ICampusService _service;

        public CampusController(ICampusService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult> GetAllAsync()
        {
            return Ok(await _service.GetAllAsync());
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
        public async Task<ActionResult> AddAsync(CampusDto model)
        {
            return StatusCodeResult(await _service.AddAsync(model));
        }
        [HttpPost]
        public async Task<ActionResult> AddRangeAsync(List<CampusDto> model)
        {
            return StatusCodeResult(await _service.AddRangeAsync(model));
        }
        [HttpPut]
        public async Task<ActionResult> UpdateAsync(CampusDto model)
        {
            return StatusCodeResult(await _service.UpdateAsync(model));
        }
        [HttpDelete]
        public async Task<ActionResult> DeleteAsync(int id)
        {
            return StatusCodeResult(await _service.DeleteAsync(id));
        }
    }
            
    //[HttpGet]
        //public async Task<ActionResult> GetAudit(decimal id)
        //{
        //    return Ok(await _service.GetAudit(id));
        //}
    }
