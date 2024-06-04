using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NET1806_LittleJoy.Repository.Commons;
using NET1806_LittleJoy.Service.Services;
using NET1806_LittleJoy.Service.Services.Interface;

namespace NET1806_LittleJoy.API.Controllers
{
    [Route("api/[controller]")]
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
            //try
            //{
            //    var result;
            //}catch (Exception ex)
            //{

            //}
            return Ok();
        }
    }
}
