using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NET1806_LittleJoy.API.ViewModels.ResponeModels;
using NET1806_LittleJoy.Service.BusinessModels;
using NET1806_LittleJoy.Service.Services.Interface;

namespace NET1806_LittleJoy.API.Controllers
{
    [Route("api/vnpay")]
    [ApiController]
    public class VNPayController : ControllerBase
    {
        private readonly IVNPayService _vnpayservice;

        public VNPayController(IVNPayService vnpayservice) 
        {
            _vnpayservice = vnpayservice;
        }

        [HttpPost]
        public async Task<IActionResult> RequestVNPay(int ordercode, int price)
        {
            try
            {
                string url = _vnpayservice.RequestVNPay(ordercode, price, HttpContext);
                return Ok(url);
            }catch (Exception ex)
            {
                return BadRequest(new ResponseModels
                {
                    HttpCode = StatusCodes.Status400BadRequest,
                    Message = ex.Message
                });
            }
        }

        [HttpGet("return")]
        public async Task<IActionResult> VNPayReturn([FromQuery] VNPayModel model)
        {
            try
            {
                var result = await _vnpayservice.ReturnFromVNPay(model);
                return Ok(result);
            }
            catch (Exception ex) 
            {
                return BadRequest(new ResponseModels
                {
                    HttpCode = StatusCodes.Status400BadRequest,
                    Message = ex.Message
                });
            }
        }
    }
}
