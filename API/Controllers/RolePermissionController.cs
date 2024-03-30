using BookingApp.DTO;
using BookingApp.Helpers;
using BookingApp.Models;
using BookingApp.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace BookingApp.Controllers
{
    public class RolePermissionController : ApiControllerBase
    {
        private readonly IRolePermissionService _rolePermissionService;
        public RolePermissionController(IRolePermissionService rolePermissionService)
        {
            _rolePermissionService = rolePermissionService;
        }
        [HttpGet]
        public async Task<ActionResult> GetAllAsync()
        {
            return Ok(await _rolePermissionService.GetAllAsync());
        }
        [HttpGet]
        public async Task<ActionResult> GetByIDAsync(int id)
        {
            return Ok(await _rolePermissionService.GetByIDAsync(id));
        }
        [HttpGet]
        public async Task<ActionResult> GetWithPaginationsAsync([FromQuery] PaginationParams paramater)
        {
            return Ok(await _rolePermissionService.GetWithPaginationsAsync(paramater));
        }
        [HttpPut]
        public async Task<ActionResult> Disable(int id)
        {
            return StatusCodeResult(await _rolePermissionService.DisableAsync(id));
        }
        [HttpPut]
        public async Task<ActionResult> UpdateAsync([FromBody] RolePermissionDto model)
        {
            return StatusCodeResult(await _rolePermissionService.UpdateAsync(model));
        }
        [HttpPost]
        public async Task<ActionResult> AddAsync([FromBody] RolePermissionDto model)
        {
            return StatusCodeResult(await _rolePermissionService.AddAsync(model));
        }
        [HttpDelete]
        public async Task<ActionResult> DeleteAsync(int id)
        {
            return StatusCodeResult(await _rolePermissionService.DeleteAsync(id));
        }


    }
}
