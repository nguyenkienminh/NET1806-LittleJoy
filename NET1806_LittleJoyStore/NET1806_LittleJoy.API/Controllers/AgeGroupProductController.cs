using Microsoft.AspNetCore.Mvc;
using NET1806_LittleJoy.Service.Services.Interface;
using Microsoft.AspNetCore.Http;
using NET1806_LittleJoy.API.ViewModels.ResponeModels;
using NET1806_LittleJoy.Repository.Commons;
using NET1806_LittleJoy.Service.Services;
using Newtonsoft.Json;
namespace NET1806_LittleJoy.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AgeGroupProductController : ControllerBase
    {
        private readonly IAgeGroupProductService _ageGroupService;

        public AgeGroupProductController(IAgeGroupProductService ageGroupProductService)
        {
            _ageGroupService = ageGroupProductService;
        }


        [HttpGet("GetAllAgeGroup")]
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


        //[Authorize(Roles = "STAFF,ADMIN")]
        [HttpGet("{getAgeGroupDetailById}")]
        public async Task<IActionResult> GetBrandDetailByIdAsync(int getAgeGroupDetailById)
        {
            try
            {
                var AgeGroupModel = await _ageGroupService.GetAgeGroupByIdAsync(getAgeGroupDetailById);

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

    }
}
