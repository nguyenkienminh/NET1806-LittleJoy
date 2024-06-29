using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NET1806_LittleJoy.API.ViewModels.RequestModels;
using NET1806_LittleJoy.API.ViewModels.ResponeModels;
using NET1806_LittleJoy.Repository.Commons;
using NET1806_LittleJoy.Service.Services.Interface;
using Newtonsoft.Json;

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

        [HttpGet("get-orders/{id}")]
        public async Task<IActionResult> GetOrderByUserId([FromQuery]PaginationParameter paginationParameter, int id)
        {
            try
            {
                var result = await _orderService.GetOrderByUserId(paginationParameter, id);
                if (result != null)
                {
                    var metadata = new
                    {
                        result.TotalCount,
                        result.PageSize,
                        result.CurrentPage,
                        result.TotalPages,
                        result.HasNext,
                        result.HasPrevious
                    };
                    Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metadata));
                    return Ok(result);
                }
                else
                {
                    return NotFound(new ResponseModels
                    {
                        HttpCode = StatusCodes.Status404NotFound,
                        Message = "Order is empty"
                    });
                }
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


        /// <summary>
        /// Delivery (1 - Chuẩn Bị, 2 - Đang Giao Hàng, 3 - Giao Hàng Thất Bại, 4 - Giao Hàng Thành Công)
        /// </summary>
        [HttpPut("update-order-delivery")]
        public async Task<IActionResult> UpdateOrderDelivery([FromBody] OrderUpdateRequestModel model)
        {
            try
            {
                var result = await _orderService.UpdateOrderDelivery(model);
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

        /// <summary>
        /// Status (1 - Đặt Hàng Thành Công, 2 - Đã Hủy)
        /// </summary>
        [HttpPut("update-order-status")]
        public async Task<IActionResult> UpdateOrderStatus([FromBody] OrderUpdateRequestModel model)
        {
            try
            {
                var result = await _orderService.UpdateOrderStatus(model);
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

        [HttpGet("get-order-by-orderCode/{orderCode}")]
        public async Task<IActionResult> GetOrderByOrderCode(int orderCode)
        {
            try
            {
                var result = await _orderService.GetOrderByOrderCode(orderCode); 
                return Ok(result);
            }catch(Exception ex)
            {
                var responseModel = new ResponseModels()
                {
                    HttpCode = StatusCodes.Status400BadRequest,
                    Message = ex.Message.ToString()
                };
                return BadRequest(responseModel);
            }
        }

        //[HttpGet("get-orders")]
        //public async Task<IActionResult> ProductFilter(OrderFilterModel model)
        //{
        //    try
        //    {
        //        return Ok();
        //    }
        //    catch (Exception ex)
        //    {
        //        var responseModel = new ResponseModels()
        //        {
        //            HttpCode = StatusCodes.Status400BadRequest,
        //            Message = ex.Message.ToString()
        //        };
        //        return BadRequest(responseModel);
        //    }
        //}
    }
}
