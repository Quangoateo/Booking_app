using Microsoft.AspNetCore.Mvc;
using BookingApp.DTO;
using BookingApp.Helpers;
using BookingApp.Services;
using Syncfusion.JavaScript;
using System.Threading.Tasks;

namespace BookingApp.Controllers
{
    public class BuildingController : ApiControllerBase
    {
        private readonly IBuildingService _service;

        public BuildingController(IBuildingService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult> GetSitesByAccount()
        {
            return Ok(await _service.GetSitesByAccount());
        }

        [HttpGet]
        public async Task<ActionResult> CheckRoom()
        {
            return Ok(await _service.CheckRoom());
        }
        //[HttpGet]
        //public async Task<ActionResult> CheckExistsitename() 
        //{
        //    return Ok(await _service.CheckExistsitename());
        //}
        [HttpPost]
        public async Task<ActionResult> DeleteUploadFile([FromForm] int key)
        {
            return Ok(await _service.DeleteUploadFile(key));
        }
        [HttpPost]
        public async Task<ActionResult> AddFormAsync([FromForm] BuildingDto model)
        {
            return Ok(await _service.AddFormAsync(model));
        }


        [HttpGet]
        public async Task<ActionResult> GetAllAsync()
        {
            return Ok(await _service.GetAllAsync());
        }

        [HttpPost]
        public async Task<ActionResult> AddAsync([FromBody] BuildingDto model)
        {
            return StatusCodeResult(await _service.AddAsync(model));
        }

        [HttpPut]
        public async Task<ActionResult> UpdateAsync([FromBody] BuildingDto model)
        {
            return StatusCodeResult(await _service.UpdateAsync(model));
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
        public async Task<ActionResult> LoadData([FromBody] DataManager request, [FromQuery] string farmGuid)
        {
            var data = await _service.LoadData(request, farmGuid);
            return Ok(data);
        }
        //[HttpGet]
        //public async Task<ActionResult> GetAudit(decimal id)
        //{
        //    return Ok(await _service.GetAudit(id));
        //}
    }
}
