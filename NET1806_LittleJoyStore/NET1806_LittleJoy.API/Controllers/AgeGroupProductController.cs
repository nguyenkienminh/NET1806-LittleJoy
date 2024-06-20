using Microsoft.AspNetCore.Mvc;
using NET1806_LittleJoy.Service.Services.Interface;
using Microsoft.AspNetCore.Http;
using NET1806_LittleJoy.API.ViewModels.ResponeModels;
using NET1806_LittleJoy.Repository.Commons;
using NET1806_LittleJoy.Service.Services;
using Newtonsoft.Json;
using NET1806_LittleJoy.API.ViewModels.RequestModels;
using NET1806_LittleJoy.Service.BusinessModels;
namespace NET1806_LittleJoy.API.Controllers
{
    [Route("api/age-group-product")]
    [ApiController]
    public class AgeGroupProductController : ControllerBase
    {
        private readonly IAgeGroupProductService _ageGroupService;

        public AgeGroupProductController(IAgeGroupProductService ageGroupProductService)
        {
            _ageGroupService = ageGroupProductService;
        }


        [HttpGet]
        //[Authorize(Roles = "STAFF,ADMIN")]
        public async Task<IActionResult> GetAllAgeGroupPagingAsync([FromQuery] PaginationParameter paginationParameter)
        {
            try
            {
                var result = await _ageGroupService.GetAllAgeGroupPagingAsync(paginationParameter);

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
                        Message = "Age Group is empty"
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
        public async Task<IActionResult> GetAgeGroupDetailByIdAsync(int Id)
        {
            try
            {
                var AgeGroupModel = await _ageGroupService.GetAgeGroupByIdAsync(Id);

                if (AgeGroupModel == null)
                {
                    return NotFound(new ResponseModels()
                    {
                        HttpCode = StatusCodes.Status404NotFound,
                        Message = "Age does not exist"
                    });
                }
                return Ok(AgeGroupModel);
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
        public async Task<IActionResult> AddAgeGroupAsync([FromBody] AgeGroupProductRequestModel ageGroupModel)
        {
            try
            {
                AgeGroupProductModel ageGroupProductModel = new AgeGroupProductModel()
                {
                    AgeRange = ageGroupModel.AgeRange,
                };

                var result = await _ageGroupService.AddAgeGroupAsync(ageGroupProductModel);

                if (result == false)
                {
                    return NotFound(new ResponseModels()
                    {
                        HttpCode = StatusCodes.Status404NotFound,
                        Message = "Can not add this Age"
                    });
                }

                else
                {
                    return Ok(new ResponseModels()
                    {
                        HttpCode = StatusCodes.Status201Created,
                        Message = "Create Age success"
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
        public async Task<IActionResult> RemoveAgeGroupByIdAsync(int Id)
        {
            try
            {
                var result = await _ageGroupService.RemoveAgeGroupByIdAsync(Id);

                if (result)
                {
                    return Ok(new ResponseModels()
                    {
                        HttpCode = StatusCodes.Status200OK,
                        Message = "Remove Age success"
                    });
                }
                else
                {
                    return NotFound(new ResponseModels()
                    {
                        HttpCode = StatusCodes.Status404NotFound,
                        Message = "This Age does not exist"
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
        public async Task<IActionResult> UpdateAgeGroupAsync([FromBody] AgeGroupProductModel ageGroupModel)
        {
            try
            {
                AgeGroupProductModel AgeModelAdd = new AgeGroupProductModel()
                {
                    Id = ageGroupModel.Id,
                    AgeRange = ageGroupModel.AgeRange,
                    
                };

                var result = await _ageGroupService.UpdateAgeGroupAsync(AgeModelAdd);

                if (result == null)
                {
                    return NotFound(new ResponseModels()
                    {
                        HttpCode = StatusCodes.Status404NotFound,
                        Message = "Can not update this Age"
                    });
                }

                return Ok(new ResponseModels()
                {
                    HttpCode = StatusCodes.Status200OK,
                    Message = "Update Age success"
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
