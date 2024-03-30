
using BookingApp.Helpers;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace BookingApp.Controllers
{
    public class MailController : ApiControllerBase
    {
        private readonly IMailExtension _service;

        public MailController(IMailExtension service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult> Index(string email, string subject, string message)
        {
            await _service.SendEmailAsync(email, subject, message);
            return Ok();
        }
    }
}
