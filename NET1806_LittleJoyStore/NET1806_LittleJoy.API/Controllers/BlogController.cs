using Microsoft.AspNetCore.Authorization;
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
    [Route("api/blog")]
    [ApiController]
    public class BlogController : ControllerBase
    {
        private readonly IBlogService _blogService;

        public BlogController(IBlogService blogService) 
        {
            _blogService = blogService;
        }

        [HttpGet]
        public async Task<IActionResult> GetListBlog([FromQuery] PaginationParameter paginationParameter)
        {
            try
            {
                var result = await _blogService.GetListBlogAsync(paginationParameter);
                if(result != null)
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
                        Message = "Blog is empty"
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

        [HttpGet("{id}")]
        public async Task<IActionResult> GetBlogById(int id)
        {
            try
            {
                var result = await _blogService.GetBlogByIdAsync(id);
                if(result == null)
                {
                    return NotFound(new ResponseModels
                    {
                        HttpCode = StatusCodes.Status404NotFound,
                        Message = "Blog not found"
                    });
                }
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

        [HttpPost]
        [Authorize(Roles = "STAFF, ADMIN")]
        public async Task<IActionResult> CreateNewBlog(BlogRequestModel blog)
        {
            try
            {
                BlogModel model = new BlogModel()
                {
                    UserId = blog.UserId,
                    Banner = blog.Banner,
                    Content = blog.Content,
                    Title = blog.Title,
                };
                var result = await _blogService.CreateNewBlogAsync(model);
                if (result == null)
                {
                    return BadRequest(new ResponseModels
                    {
                        HttpCode = StatusCodes.Status400BadRequest,
                        Message = "Can not add this blog"
                    });
                }
                else
                {
                    return Ok(result);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseModels
                {
                    HttpCode = StatusCodes.Status400BadRequest,
                    Message = ex.Message.ToString(),
                });
            }
        }

        [HttpPut]
        [Authorize(Roles = "STAFF, ADMIN")]
        public async Task<IActionResult> UpdateBlog(BlogRequestUpdateModel blog)
        {
            try
            {
                var blogModel = new BlogModel()
                {
                    Banner = blog.Banner,
                    Content = blog.Content,
                    Title = blog.Title,
                    Date = DateTime.UtcNow.AddHours(7),
                    Id = blog.Id,
                    UserId = blog.UserId,
                };
                var result = await _blogService.UpdateBlogAsync(blogModel);
                if (result == null)
                {
                    return BadRequest(new ResponseModels
                    {
                        HttpCode = StatusCodes.Status400BadRequest,
                        Message = "Can not update this blog",
                    });
                }
                else
                {
                    return Ok(result);
                }
            }
            catch(Exception ex)
            {
                return BadRequest(new ResponseModels
                {
                    HttpCode = StatusCodes.Status400BadRequest,
                    Message = ex.Message.ToString(),
                });
            }
        }

        [HttpDelete]
        [Authorize(Roles = "STAFF, ADMIN")]
        public async Task<IActionResult> RemoveBlog(int id)
        {
            try
            {
                var result = await _blogService.RemoveBlogAsync(id);
                if (result)
                {
                    return Ok(new ResponseModels
                    {
                        HttpCode = StatusCodes.Status200OK,
                        Message = "Remove blog successfully"
                    });
                }

                return BadRequest(new ResponseModels
                {
                    HttpCode = StatusCodes.Status400BadRequest,
                    Message = "Can not remove blog"
                });
            }catch(Exception ex)
            {
                return BadRequest(new ResponseModels
                {
                    HttpCode = StatusCodes.Status400BadRequest,
                    Message = ex.Message,
                });
            }
        }

        /// <summary>
        /// SortDate (1 - Ascending, 2 - Descending)
        /// </summary>
        [HttpGet("filter")]
        [Authorize(Roles = "STAFF, ADMIN")]
        public async Task<IActionResult> GetListBlogFilter([FromQuery] BlogFilterModel filter, [FromQuery] PaginationParameter paging)
        {
            try
            {
                var result = await _blogService.GetListBlogFilterAsync(paging, filter);
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
                        Message = "Blog is empty"
                    });
                }
            }
            catch(Exception ex)
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
