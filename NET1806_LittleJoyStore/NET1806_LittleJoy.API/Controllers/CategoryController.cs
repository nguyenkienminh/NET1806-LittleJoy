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
    [Route("api/category")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoryController( ICategoryService categoryService)
        {
            _categoryService = categoryService; 
        }

        [HttpGet]
        //[Authorize(Roles = "STAFF,ADMIN")]
        public async Task<IActionResult> GetAllCategoryPagingAsync([FromQuery] PaginationParameter paginationParameter)
        {
            try
            {
                var result = await _categoryService.GetAllCategoryPagingAsync(paginationParameter);

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
                        Message = "Category is empty"
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
        public async Task<IActionResult> GetCategoryByIdAsync(int Id)
        {
            try
            {
                var CategoryModel = await _categoryService.GetCategoryByIdAsync(Id);

                if (CategoryModel == null)
                {
                    return NotFound(new ResponseModels()
                    {
                        HttpCode = StatusCodes.Status404NotFound,
                        Message = "Category does not exist"
                    });
                }
                return Ok(CategoryModel);
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
        public async Task<IActionResult> AddCategoryAsync([FromBody] CategoryRequestModel categoryRequestModel)
        {
            try
            {
                CategoryModel cateModelAdd = new CategoryModel()
                {
                    CategoryName = categoryRequestModel.CategoryName,
                };

                var result = await _categoryService.AddCategoryAsync(cateModelAdd);

                if (result == false)
                {
                    return NotFound(new ResponseModels()
                    {
                        HttpCode = StatusCodes.Status404NotFound,
                        Message = "Can not add this category"
                    });
                }

                else
                {
                    return Ok(new ResponseModels()
                    {
                        HttpCode = StatusCodes.Status201Created,
                        Message = "Create category success"
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
        public async Task<IActionResult> RemoveCategoryByIdAsync(int Id)
        {
            try
            {
                var result = await _categoryService.RemoveCategoryByIdAsync(Id);

                if (result)
                {
                    return Ok(new ResponseModels()
                    {
                        HttpCode = StatusCodes.Status200OK,
                        Message = "Remove category success"
                    });
                }
                else
                {
                    return NotFound(new ResponseModels()
                    {
                        HttpCode = StatusCodes.Status404NotFound,
                        Message = "The category can not remove"
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
        public async Task<IActionResult> UpdateCategoryAsync([FromBody] CategoryModel cateModel)
        {
            try
            {
                CategoryModel cateModelAdd = new CategoryModel()
                {
                    Id = cateModel.Id,
                    CategoryName = cateModel.CategoryName, 
                };

                var result = await _categoryService.UpdateCategoryAsync(cateModelAdd);

                if (result == null)
                {
                    return NotFound(new ResponseModels()
                    {
                        HttpCode = StatusCodes.Status404NotFound,
                        Message = "Can not update this Category"
                    });
                }

                return Ok(new ResponseModels()
                {
                    HttpCode = StatusCodes.Status200OK,
                    Message = "Update category success"
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
