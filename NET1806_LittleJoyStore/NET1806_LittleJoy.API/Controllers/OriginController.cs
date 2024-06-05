using Microsoft.AspNetCore.Http;
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
    [Route("api/origin")]
    [ApiController]
    public class OriginController : ControllerBase
    {
        private readonly IOriginService _originService;

        public OriginController(IOriginService originService)
        {
            _originService = originService;
        }

        [HttpGet]
        //[Authorize(Roles = "STAFF,ADMIN")]
        public async Task<IActionResult> GetAllOriginPagingAsync([FromQuery] PaginationParameter paginationParameter)
        {
            try
            {
                var result = await _originService.GetAllOriginPagingAsync(paginationParameter);

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
                        Message = "Origins are empty"
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


        //[Authorize(Roles = "STAFF,ADMIN")]
        [HttpGet("{Id}")]
        public async Task<IActionResult> GetOriginByIdAsync(int Id)
        {
            try
            {
                var origin = await _originService.GetOriginByIdAsync(Id);

                if (origin == null)
                {
                    return NotFound(new ResponseModels()
                    {
                        HttpCode = StatusCodes.Status404NotFound,
                        Message = "Brand does not exist"
                    });
                }
                return Ok(origin);
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
        //[Authorize(Roles = "STAFF,ADMIN")]
        public async Task<IActionResult> AddOriginAsync([FromBody] OriginRequestModel originRequestModel)
        {
            try
            {
                OriginModel originAdd = new OriginModel()
                {
                    OriginName = originRequestModel.OriginName,
                };

                var result = await _originService.AddOriginAsync(originAdd);

                if (result == false)
                {
                    return NotFound(new ResponseModels()
                    {
                        HttpCode = StatusCodes.Status404NotFound,
                        Message = "Can not add this Origin"
                    });
                }

                else
                {
                    return Ok(new ResponseModels()
                    {
                        HttpCode = StatusCodes.Status201Created,
                        Message = "Create Origin success"
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
        public async Task<IActionResult> RemoveOriginByIdAsync(int Id)
        {
            try
            {
                var result = await _originService.RemoveOriginByIdAsync(Id);

                if (result)
                {
                    return Ok(new ResponseModels()
                    {
                        HttpCode = StatusCodes.Status200OK,
                        Message = "Remove origin success"
                    });
                }
                else
                {
                    return NotFound(new ResponseModels()
                    {
                        HttpCode = StatusCodes.Status404NotFound,
                        Message = "The origin can not remove"
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
        //[Authorize(Roles = "STAFF,ADMIN")]
        public async Task<IActionResult> UpdateOriginAsync([FromBody] OriginModel originModel)
        {
            try
            {
                OriginModel originModelAdd = new OriginModel()
                {
                    Id = originModel.Id,
                    OriginName = originModel.OriginName,
                };

                var result = await _originService.UpdateOriginAsync(originModel);

                if (result == null)
                {
                    return NotFound(new ResponseModels()
                    {
                        HttpCode = StatusCodes.Status404NotFound,
                        Message = "Can not update this Origin"
                    });
                }

                return Ok(new ResponseModels()
                {
                    HttpCode = StatusCodes.Status200OK,
                    Message = "Update origin success"
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
    }
}
