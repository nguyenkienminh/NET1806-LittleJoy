using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using NET1806_LittleJoy.API.ViewModels.RequestModels;
using NET1806_LittleJoy.API.ViewModels.ResponeModels;
using NET1806_LittleJoy.Repository.Commons;
using NET1806_LittleJoy.Service.BusinessModels;
using NET1806_LittleJoy.Service.Services;
using NET1806_LittleJoy.Service.Services.Interface;
using Newtonsoft.Json;

namespace NET1806_LittleJoy.API.Controllers
{
    [Route("api/feedback")]
    [ApiController]
    public class FeedBackController : ControllerBase
    {
        private readonly IFeedBackService _service;

        public FeedBackController(IFeedBackService service)
        {
            _service = service;
        }

        [HttpGet]
        //[Authorize(Roles = "STAFF,ADMIN")]
        public async Task<IActionResult> GetAllFeedBackPagingAsync([FromQuery] PaginationParameter paginationParameter)
        {
            try
            {
                var result = await _service.GetAllFeedBackPagingAsync(paginationParameter);

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
                        Message = "FeedBack is empty"
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


        [HttpGet("{Id}")]
        //[Authorize(Roles = "STAFF,ADMIN")]
        public async Task<IActionResult> GetFeedBackByIdAsync(int Id)
        {
            try
            {
                var detailModel = await _service.GetFeedBackByIdAsync(Id);

                if (detailModel == null)
                {
                    return NotFound(new ResponseModels()
                    {
                        HttpCode = StatusCodes.Status404NotFound,
                        Message = "Feedback does not exist"
                    });
                }
                return Ok(detailModel);
            }
            catch (Exception ex)
            {

                return BadRequest(new ResponseModels()
                {
                    HttpCode = StatusCodes.Status400BadRequest,
                    Message = ex.Message.ToString()
                });
            }
        }


        [HttpPost]
        //[Authorize(Roles = "STAFF,ADMIN,USER")]
        public async Task<IActionResult> AddFeedBackAsync([FromBody] FeedBackRequestModel request)
        {
            try
            {
                FeedBackModel ModelAdd = new FeedBackModel()
                {
                    ProductId = request.ProductId,
                    UserId = request.UserId,
                    Rating = request.Rating,
                    Comment = request.Comment,
                };

                var result = await _service.AddFeedBackAsync(ModelAdd);

                if (result == false)
                {
                    return NotFound(new ResponseModels()
                    {
                        HttpCode = StatusCodes.Status404NotFound,
                        Message = "Can not create this feedback"
                    });
                }

                else
                {
                    return Ok(new ResponseModels()
                    {
                        HttpCode = StatusCodes.Status201Created,
                        Message = "Create FeedBack success"
                    });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseModels()
                {
                    HttpCode = StatusCodes.Status400BadRequest,
                    Message = ex.Message.ToString()
                });
            }
        }


        [HttpDelete]
        //[Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> RemoveFeedBackByIdAsync(int Id)
        {
            try
            {
                var result = await _service.RemoveFeedBackByIdAsync(Id);

                if (result)
                {
                    return Ok(new ResponseModels()
                    {
                        HttpCode = StatusCodes.Status200OK,
                        Message = "Remove Feedback success"
                    });
                }
                else
                {
                    return NotFound(new ResponseModels()
                    {
                        HttpCode = StatusCodes.Status404NotFound,
                        Message = "The feedback can not remove"
                    });
                }
            }
            catch (Exception ex)
            {

                return BadRequest(new ResponseModels()
                {
                    HttpCode = StatusCodes.Status400BadRequest,
                    Message = ex.Message.ToString()
                });
            }
        }


        [HttpPut]
        //[Authorize(Roles = "STAFF,ADMIN,USER")]
        public async Task<IActionResult> UpdateFeedBackAsync([FromBody] FeedBackUpdateRequestModel model)
        {
            try
            {
                FeedBackModel feedBackModel = new FeedBackModel()
                {
                    Id = model.Id,
                    Comment = model.Comment,
                    Rating = model.Rating,  
                };

                var result = await _service.UpdateFeedBackAsync(feedBackModel);

                if (result == null)
                {
                    return NotFound(new ResponseModels()
                    {
                        HttpCode = StatusCodes.Status404NotFound,
                        Message = "Can not feedback this Brand"
                    });
                }

                return Ok(new ResponseModels()
                {
                    HttpCode = StatusCodes.Status200OK,
                    Message = "Update feedback success"
                });
            }
            catch (Exception ex)
            {

                return BadRequest(new ResponseModels()
                {
                    HttpCode = StatusCodes.Status400BadRequest,
                    Message = ex.Message.ToString()
                });
            }
        }


        [HttpGet("product-average-point/{Id}")]
        //[Authorize(Roles = "STAFF,ADMIN")]
        public async Task<IActionResult> GetProductAveragePoint(int Id)
        {
            try
            {
                var pointModel = await _service.AverageFeedBackInProduct(Id);

                if (pointModel == null)
                {
                    return NotFound(new ResponseModels()
                    {
                        HttpCode = StatusCodes.Status404NotFound,
                        Message = "Feedback does not exist"
                    });
                }
                return Ok(pointModel);
            }
            catch (Exception ex)
            {

                return BadRequest(new ResponseModels()
                {
                    HttpCode = StatusCodes.Status400BadRequest,
                    Message = ex.Message.ToString()
                });
            }
        }


        [HttpGet("feed-back-by-product/{Id}")]
        //[Authorize(Roles = "STAFF,ADMIN")]
        public async Task<IActionResult> GetFeedBackByProductIdAsync([FromQuery] PaginationParameter paginationParameter, int Id)
        {
            try
            {
                var result = await _service.GetFeedBackByProductIdAsync(Id,paginationParameter);

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
                        Message = "FeedBack is empty"
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
    }
}
