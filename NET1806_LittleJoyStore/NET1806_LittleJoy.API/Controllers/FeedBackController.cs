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
        private readonly IUserService _userService;

        public FeedBackController(IFeedBackService service, IUserService userService)
        {
            _service = service;
            _userService = userService;
        }

        [HttpGet]
        //[Authorize(Roles = "STAFF,ADMIN")]
        public async Task<IActionResult> GetAllFeedBackPagingAsync([FromQuery] PaginationParameter paginationParameter)
        {
            try
            {
                var model = await _service.GetAllFeedBackPagingAsync(paginationParameter);


                if (model == null)
                {
                    return NotFound(new ResponseModels
                    {
                        HttpCode = StatusCodes.Status404NotFound,
                        Message = "FeedBack is empty"
                    });
                }

                var result = new Pagination<FeedBackResponseModel>(
                    model.Select(f => new FeedBackResponseModel
                    {
                        Id = f.Id,
                        ProductId = f.ProductId,
                        Comment = f.Comment,
                        Date = f.Date,
                        Rating = f.Rating,
                        UserId = f.UserId,
                        UserName = _userService.GetUserByIdAsync(f.UserId).Result.Fullname,

                    }).ToList(),
                    model.TotalCount,
                    model.CurrentPage,
                    model.PageSize);

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
                else
                {
                    FeedBackResponseModel result = new FeedBackResponseModel()
                    {
                        Id = detailModel.Id,
                        ProductId = detailModel.ProductId,
                        Comment = detailModel.Comment,
                        Date = detailModel.Date,
                        Rating = detailModel.Rating,
                        UserId = detailModel.UserId,
                        UserName = _userService.GetUserByIdAsync(detailModel.UserId).Result.Fullname,

                    };
                    return Ok(result);
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
        public async Task<IActionResult> RemoveFeedBackByIdAsync(int Id, int UserId)
        {
            try
            {
                var result = await _service.RemoveFeedBackByIdAsync(Id, UserId);

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
                    UserId = model.UserId,
                    Comment = model.Comment,
                    Rating = model.Rating,
                };

                var result = await _service.UpdateFeedBackAsync(feedBackModel);

                if (result == null)
                {
                    return NotFound(new ResponseModels()
                    {
                        HttpCode = StatusCodes.Status404NotFound,
                        Message = "Can not update this feedback"
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
                var model = await _service.GetFeedBackByProductIdAsync(Id, paginationParameter);


                if (model == null)
                {
                    return NotFound(new ResponseModels
                    {
                        HttpCode = StatusCodes.Status404NotFound,
                        Message = "FeedBack is empty"
                    });
                }

                var result = new Pagination<FeedBackResponseModel>(
                    model.Select(f => new FeedBackResponseModel
                    {
                        Id = f.Id,
                        ProductId = f.ProductId,
                        Comment = f.Comment,
                        Date = f.Date,
                        Rating = f.Rating,
                        UserId = f.UserId,
                        UserName = _userService.GetUserByIdAsync(f.UserId).Result.Fullname,

                    }).ToList(),
                    model.TotalCount,
                    model.CurrentPage,
                    model.PageSize);

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


        [HttpGet("count-feedback-by-product/{Id}")]
        //[Authorize(Roles = "STAFF,ADMIN")]
        public async Task<IActionResult> CountFeedBackByProductIdAsync(int Id)
        {
            try
            {
                var count = await _service.CountFeedBackByProductAsync(Id);

                if (count == 0)
                {
                    return NotFound(new ResponseModels()
                    {
                        HttpCode = StatusCodes.Status404NotFound,
                        Message = "Feedback không tồn tại"
                    });
                }
                return Ok(count);
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
    }
}
