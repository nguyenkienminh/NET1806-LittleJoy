using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NET1806_LittleJoy.API.ViewModels.RequestModels;
using NET1806_LittleJoy.API.ViewModels.ResponeModels;
using NET1806_LittleJoy.Service.Services.Interface;

namespace NET1806_LittleJoy.API.Controllers
{
    [Route("api/order")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;
        private readonly IUserService _userService;

        public OrderController(IOrderService orderService, IUserService userService) 
        {
            _orderService = orderService;
            _userService = userService;
        }

        /// <summary>
        /// Payment Method (1 - COD, 2 - VNPAY)
        /// </summary>
        [HttpPost("create-order")]
        public async Task<IActionResult> CreateNewOrder([FromBody] OrderRequestModel request)
        {
            try
            {
                var result = await _orderService.CreateOrder(request, HttpContext);
                return Ok(result);
            }
            catch (Exception ex)
            {
                var responseModel = new ResponseModels()
                {
                    HttpCode = StatusCodes.Status400BadRequest,
                    Message = ex.Message.ToString()
                };
                return BadRequest(responseModel);
            }
        }

        [HttpPut]
        public async Task<IActionResult> UpdateOrder([FromBody] OrderRequestModel request)
        {
            try
            {
                return Ok();
            }
            catch (Exception ex) 
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
