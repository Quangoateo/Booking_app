using BookingApp.DTO;
using BookingApp.Helpers;
using BookingApp.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BookingApp.Controllers
{
    public class RoleController : ApiControllerBase
    {
        private readonly IRoleService _service;
        public RoleController(IRoleService service)
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
        public async Task<ActionResult> GetWithPaginationsAsync([FromQuery] PaginationParams paramater)
        {
            return Ok(await _service.GetWithPaginationsAsync(paramater));
        }
        [HttpPut]
        public async Task<ActionResult> Disable(int id)
        {
            return StatusCodeResult(await _service.DisableAsync(id));
        }
        [HttpPut]
        public async Task<ActionResult> UpdateAsync([FromBody] RoleDto model)
        {
            return StatusCodeResult(await _service.UpdateAsync(model));
        }
        [HttpPost]
        public async Task<ActionResult> AddAsync([FromBody] RoleDto model)
        {
            return StatusCodeResult(await _service.AddAsync(model));
        }
        [HttpPost]
        public async Task<ActionResult> AddRangeAsync([FromBody] List<RoleDto> models)
        {
            return StatusCodeResult(await _service.AddRangeAsync(models));
        }
        [HttpDelete]
        public async Task<ActionResult> DeleteAsync(int id)
        {
            return StatusCodeResult(await _service.DeleteAsync(id));
        }
    }
}
