using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NET1806_LittleJoy.API.ViewModels.RequestModels;
using NET1806_LittleJoy.API.ViewModels.ResponeModels;
using NET1806_LittleJoy.Repository.Commons;
using NET1806_LittleJoy.Repository.Entities;
using NET1806_LittleJoy.Service.BusinessModels;
using NET1806_LittleJoy.Service.Services;
using NET1806_LittleJoy.Service.Services.Interface;
using Newtonsoft.Json;

namespace NET1806_LittleJoy.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BrandController : ControllerBase
    {
        private readonly IBrandService _brandService;

        public BrandController(IBrandService brandService)
        {
            _brandService = brandService;
        }

        [HttpGet("GetAllBrand")]
        //[Authorize(Roles = "STAFF,ADMIN")]
        public async Task<IActionResult> GetAllBrandPagingAsync([FromQuery] PaginationParameter paginationParameter)
        {
            try
            {
                var result = await _brandService.GetAllBrandPagingAsync(paginationParameter);

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
                        Message = "Brand is empty"
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
        [HttpGet("{getBrandDetailById}")]
        public async Task<IActionResult> GetBrandDetailByIdAsync(int getBrandDetailById)
        {
            try
            {
                var brandDetailModel = await _brandService.GetBrandByIdAsync(getBrandDetailById);

                if (brandDetailModel == null)
                {
                    return NotFound(new ResponseModels()
                    {
                        HttpCode = StatusCodes.Status404NotFound,
                        Message = "Brand does not exist"
                    });
                }
                return Ok(brandDetailModel);
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


        [HttpPost("AddBrand")]
        //[Authorize(Roles = "STAFF,ADMIN")]
        public async Task<IActionResult> AddBrandAsync([FromBody] BrandRequestModel brandRequestModel)
        {
            try
            {
                BrandModel brandModelAdd = new BrandModel()
                {
                    BrandName = brandRequestModel.BrandName,
                    BrandDescription = brandRequestModel.BrandDescription,
                    Logo = brandRequestModel.Logo,
                };

                var result = await _brandService.AddBrandAsync(brandModelAdd);

                if (result == false)
                {
                    return NotFound(new ResponseModels()
                    {
                        HttpCode = StatusCodes.Status404NotFound,
                        Message = "Can not add this brand"
                    });
                }

                else
                {
                    return Ok(new ResponseModels()
                    {
                        HttpCode = StatusCodes.Status201Created,
                        Message = "Create Brand success"
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


        //[Authorize(Roles = "ADMIN")]
        [HttpDelete("{removeBrandById}")]
        public async Task<IActionResult> RemoveBrandByIdAsync(int removeBrandById)
        {
            try
            {
                var result = await _brandService.RemoveBrandAsync(removeBrandById);

                if (result)
                {
                    return Ok(new ResponseModels()
                    {
                        HttpCode = StatusCodes.Status200OK,
                        Message = "Remove brand success"
                    });
                }
                else
                {
                    return NotFound(new ResponseModels()
                    {
                        HttpCode = StatusCodes.Status404NotFound,
                        Message = "The brand does not exist"
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


        //[Authorize(Roles = "STAFF,ADMIN")]
        [HttpPut("UpdateBrand")]
        public async Task<IActionResult> UpdateBrandAsync([FromBody] BrandRequestModel brandRequestModel)
        {
            try
            {
                BrandModel brandModel = new BrandModel()
                {
                    Id = brandRequestModel.Id,
                    Logo = brandRequestModel.Logo,
                    BrandDescription = brandRequestModel.BrandDescription,
                    BrandName = brandRequestModel.BrandName 
                };

                var result = await _brandService.UpdateBrandAsync(brandModel);

                if (result == null)
                {
                    return NotFound(new ResponseModels()
                    {
                        HttpCode = StatusCodes.Status404NotFound,
                        Message = "Can not update this Brand"
                    });
                }

                return Ok(new ResponseModels()
                {
                    HttpCode = StatusCodes.Status200OK,
                    Message = "Update brand success"
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
