using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using NET1806_LittleJoy.API.ViewModels.RequestModels;
using NET1806_LittleJoy.API.ViewModels.ResponeModels;
using NET1806_LittleJoy.Repository.Commons;
using NET1806_LittleJoy.Service.BusinessModels;
using NET1806_LittleJoy.Service.Services.Interface;
using Newtonsoft.Json;

namespace NET1806_LittleJoy.API.Controllers
{
    [Route("api/user")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _service;

        public UserController(IUserService service)
        {
            _service = service;
        }


        [HttpGet]
        //[Authorize(Roles = "STAFF,ADMIN")]
        public async Task<IActionResult> GetAllUserByRoleIdAsync([FromQuery] PaginationParameter paging, int roleId)
        {
            try
            {
                var model = await _service.GetAllPagingUserByRoleIdAsync(paging, roleId);

                var result = new Pagination<UserResponseModel>(
                    model.Select(a => new UserResponseModel
                    {
                        Id = a.Id,
                        UserName = a.UserName,
                        Fullname = a.Fullname,
                        RoleId = a.RoleId,
                        Avatar = a.Avatar,
                        Email = a.Email,
                        PhoneNumber = a.PhoneNumber,
                        Points = a.Points,
                        Status = a.Status,
                        UnsignName = a.UnsignName,
                        ConfirmEmail = a.ConfirmEmail,
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
                        Message = "User is empty"
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
        //[Authorize(Roles = "USER")]
        public async Task<IActionResult> GetUserByIdAsync(int Id)
        {
            try
            {
                UserResponseModel result = null;

                var userDetailModel = await _service.GetUserByIdAsync(Id);

                if (userDetailModel != null)
                {
                    result = new UserResponseModel()
                    {
                        Id = userDetailModel.Id,
                        RoleId = userDetailModel.RoleId,
                        UserName = userDetailModel.UserName,
                        Fullname = userDetailModel.Fullname,
                        Email = userDetailModel.Email,
                        PhoneNumber = userDetailModel.PhoneNumber,
                        Avatar = userDetailModel.Avatar,
                        Points = userDetailModel.Points,
                        Status = userDetailModel.Status,
                        UnsignName = userDetailModel.UnsignName,
                        ConfirmEmail = userDetailModel.ConfirmEmail,
                    };
                }

                if (result == null)
                {
                    return NotFound(new ResponseModels()
                    {
                        HttpCode = StatusCodes.Status404NotFound,
                        Message = "User does not exist"
                    });
                }
                return Ok(result);
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
        //[Authorize(Roles = "STAFF,ADMIN, USER")]
        public async Task<IActionResult> AddUserAsync([FromBody] UserRequestModel request)
        {
            try
            {
                UserModel model = new UserModel()
                {
                    UserName = request.UserName,
                    Password = request.Password,
                    RoleId = request.RoleId,
                    Fullname = request.Fullname,
                    Email = request.Email,
                    Avatar = request.Avatar,
                    PhoneNumber = request.PhoneNumber,
                };

                var result = await _service.AddUserAsync(model);

                if (result == false)
                {
                    return NotFound(new ResponseModels()
                    {
                        HttpCode = StatusCodes.Status404NotFound,
                        Message = "Can not add this User"
                    });
                }

                else
                {
                    return Ok(new ResponseModels()
                    {
                        HttpCode = StatusCodes.Status201Created,
                        Message = "Add User success"
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
    }
}
