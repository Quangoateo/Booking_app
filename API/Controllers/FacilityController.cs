using BookingApp.DTO;
using BookingApp.DTO.Filter;
using BookingApp.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace BookingApp.Controllers
{
    public class FacilityController : ApiControllerBase
    {
        private readonly IFacilityService _service;
        public FacilityController(IFacilityService service)
        {
            _service = service;
        }
        [HttpGet]
        public async Task<ActionResult> GetAllAsync()
        {
            return Ok(await _service.GetAllAsync());
        }
        [HttpPut]
        public async Task<ActionResult> UpdateAsync(FacilityDto model)
        {
            return Ok(await _service.UpdateAsync(model));
        }

        [HttpPost]
        public async Task<ActionResult> AddAsync(FacilityDto model)
        {
            return Ok(await _service.AddAsync(model));
        }
        [HttpPost]
        public async Task<ActionResult> AddRangeAsync(List<FacilityDto> model)
        {
            return Ok(await _service.AddRangeAsync(model));
        }
        [HttpDelete]
        public async Task<ActionResult> DeleteAsync(int id)
        {
            return Ok(await _service.DeleteAsync(id));
        }
        
    }
}
