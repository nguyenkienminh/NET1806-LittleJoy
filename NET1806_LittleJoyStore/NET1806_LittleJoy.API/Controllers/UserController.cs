using Microsoft.AspNetCore.Authorization;
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
using System.Collections.Generic;

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
        public async Task<IActionResult> GetAllUserByRoleIdAndStatusAsync([FromQuery] PaginationParameter paging, int roleId, bool status)
        {
            try
            {
                var model = await _service.GetAllPagingUserByRoleIdAndStatusAsync(paging, roleId, status);


                if(model == null)
                {
                    return NotFound(new ResponseModels
                    {
                        HttpCode = StatusCodes.Status404NotFound,
                        Message = "User is empty"
                    });
                }

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

                var result = await _service.AddUserAsync(model, request.MainAddress);

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


        [HttpDelete]
        //[Authorize(Roles = "STAFF,ADMIN, USER")]
        public async Task<IActionResult> DeleteUserAsyncById(int Id)
        {
            try
            {
                var result = await _service.DeleteUserByIdAsync(Id);

                if (result)
                {
                    return Ok(new ResponseModels()
                    {
                        HttpCode = StatusCodes.Status200OK,
                        Message = "Remove User success"
                    });
                }
                else
                {
                    return NotFound(new ResponseModels()
                    {
                        HttpCode = StatusCodes.Status404NotFound,
                        Message = "The user can not remove"
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
        public async Task<IActionResult> UpdateUserAsync([FromBody] UpdateUserRequestModel request)
        {
            try
            {
                UserModel modelChange = new UserModel()
                {
                    Id = request.Id,
                    PhoneNumber = request.PhoneNumber,
                    Status = request.Status,
                    Fullname = request.Fullname,
                    RoleId = request.RoleId,
                };

                var result = await _service.UpdateUserAsync(modelChange, request.MainAddress);

                if (result == null)
                {
                    return NotFound(new ResponseModels()
                    {
                        HttpCode = StatusCodes.Status404NotFound,
                        Message = "Can not update this User"
                    });
                }

                return Ok(new ResponseModels()
                {
                    HttpCode = StatusCodes.Status200OK,
                    Message = "Update User success"
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


        [HttpPut("user-role")]
        //[Authorize(Roles = "USER")]
        public async Task<IActionResult> UpdateInformationForUserRoleAsync([FromBody] UpdateUserRoleRequestModel request)
        {
            try
            {
                UserModel modelChange = new UserModel()
                {
                    Id = request.Id,
                    PhoneNumber = request.PhoneNumber,
                    Avatar = request.Avatar,
                    Fullname = request.Fullname,
                };

                var result = await _service.UpdateUserRoleAsync(modelChange);

                if (result == null)
                {
                    return NotFound(new ResponseModels()
                    {
                        HttpCode = StatusCodes.Status404NotFound,
                        Message = "Can not update this User"
                    });
                }

                return Ok(new ResponseModels()
                {
                    HttpCode = StatusCodes.Status200OK,
                    Message = "Update User success"
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


        [HttpPut("change-password")]
        public async Task<IActionResult> ChangePasswordUserRoleAsync([FromBody] UpdatePasswordUserRequestModel request)
        {
            try
            {
                if (ModelState.IsValid)
                {

                    ChangePasswordModel model = new ChangePasswordModel()
                    {
                        Id = request.Id,
                        OldPassword = request.OldPassword,
                        NewPassword = request.NewPassword,
                        ConfirmPassword = request.ConfirmPassword
                    };

                    var result = await _service.ChangePasswordUserRoleAsync(model);

                    if (result == true)
                    {
                        var resp = new ResponseModels()
                        {
                            HttpCode = StatusCodes.Status200OK,
                            Message = "Đổi mật khẩu thành công",
                        };
                        return Ok(resp);
                    }

                    else if (result == false)
                    {
                        var resp = new ResponseModels()
                        {
                            HttpCode = StatusCodes.Status400BadRequest,
                            Message = "Mật khẩu không đúng",
                        };
                        return BadRequest(resp);
                    }
                }
                return ValidationProblem(ModelState);
            } catch (Exception ex)
            {
                return BadRequest(new ResponseModels()
                {
                    HttpCode = StatusCodes.Status400BadRequest,
                    Message = ex.Message.ToString()
                });
            }
        }


        [HttpGet("highest-score")]
        //[Authorize(Roles = "STAFF,ADMIN")]
        public async Task<IActionResult> GetAllUserHighestScoreAsync()
        {
            try
            {
                var model = await _service.GetUserListHighestScoreAsync();

                var result = model.Select(a => new UserResponseModel
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
                }).ToList();

                if (result != null)
                {
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
    }
}
