
using Microsoft.AspNetCore.Mvc;
using BookingApp.DTO;

namespace BookingApp.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class ApiControllerBase: ControllerBase
    {
        [NonAction] //Set not Tracking http method
        public ObjectResult StatusCodeResult(OperationResult result)
        {
            if (result.Success)
            {
                return Ok(result);
            }
            else
            {
                //return StatusCode((int)result.StatusCode, result.Message);
                return Ok(result);
            }
        }
    }
}
