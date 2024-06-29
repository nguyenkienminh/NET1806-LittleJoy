using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NET1806_LittleJoy.API.ViewModels.ResponeModels;
using NET1806_LittleJoy.Service.Services;
using NET1806_LittleJoy.Service.Services.Interface;

namespace NET1806_LittleJoy.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PointMoneyController : ControllerBase
    {
        private readonly IPointMoneyService _pointMoneyService;

        public PointMoneyController(IPointMoneyService pointMoneyService) 
        {
            _pointMoneyService = pointMoneyService;
        }

        [HttpGet]
        public async Task<IActionResult> CheckDiscount(int userId) 
        {
            try
            {
                var result = await _pointMoneyService.CheckDiscount(userId);
                return Ok(result);
            }catch (Exception ex) 
            {
                var responseModel = new ResponseModels()
                {
                    HttpCode = StatusCodes.Status400BadRequest,
                    Message = ex.Message.ToString()
                };
                return BadRequest(responseModel);
            }
        }
    }
}
