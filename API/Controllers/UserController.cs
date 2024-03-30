using Microsoft.AspNetCore.Mvc;
using BookingApp.DTO;
using BookingApp.Helpers;
using BookingApp.Services;
using Syncfusion.JavaScript;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using BookingApp.DTO.auth;
using BookingApp.DTO.Filter;

namespace BookingApp.Controllers
{
    public class UserController : ApiControllerBase
    {
        private readonly IUserService _service;

        public UserController(IUserService service)
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
        public async Task<ActionResult> SearchUser([FromQuery] UserFilter userFilter)
        {
            return Ok(await _service.SearchUser(userFilter));
        }
        [HttpGet]
        public async Task<UserWithPermission> GetUserWithPermission(int id)
        {
            return await _service.GetUserWithPermissionAsync(id);
        }

        [HttpGet]
        public async Task<ActionResult> GetWithPaginationsAsync(PaginationParams paramater)
        {
            return Ok(await _service.GetWithPaginationsAsync(paramater));
        }
        [HttpPut]
        public async Task<ActionResult> UpdateAsync([FromBody] UserDto model)
        {
            return StatusCodeResult(await _service.UpdateAsync(model));
        }
        [HttpPut]
        public async Task<ActionResult> UpdateRole(int id, int roleId)
        {
            return StatusCodeResult(await _service.UpdateRole(id, roleId));
        }
        [HttpPost]
        public async Task<ActionResult> AddAsync([FromBody] UserDto model)
        {
            return StatusCodeResult(await _service.AddAsync(model));
        }
        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> LoginAsync([FromBody] UserForLoginDto model)
        {
            return StatusCodeResult(await _service.LoginAsync(model));
        }
        [HttpDelete]
        public async Task<ActionResult> DeleteAsync(int id)
        {
            return StatusCodeResult(await _service.DeleteAsync(id));
        }


        

    }
}
