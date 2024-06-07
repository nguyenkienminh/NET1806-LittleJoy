using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NET1806_LittleJoy.Repository.Commons;
using NET1806_LittleJoy.Service.Services.Interface;

namespace NET1806_LittleJoy.API.Controllers
{
    [Route("api/mail")]
    [ApiController]
    public class MailController : ControllerBase
    {
        private readonly IMailService _mailService;

        public MailController(IMailService mailService) { 
            _mailService = mailService;
        }
        [HttpPost("send")]
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> SendEmailAsync(MailRequest request)
        {
            try
            {
                await _mailService.sendEmailAsync(request);
                return Ok();
            }catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
