using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using NET1806_LittleJoy.API.ViewModels.RequestModels;
using NET1806_LittleJoy.API.ViewModels.ResponeModels;
using NET1806_LittleJoy.Service.BusinessModels;
using NET1806_LittleJoy.Service.Services.Interface;

namespace NET1806_LittleJoy.API.Controllers
{
    [Route("api/authen")]
    [ApiController]
    public class AuthenController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IOtpService _otpService;

        public AuthenController(IUserService userService, IOtpService otpService)
        {
            _userService = userService;
            _otpService = otpService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> LoginWithUsername(LoginRequestModel model)
        {
            try
            {
                var result = await _userService.LoginByUsernameAndPassword(model.Username, model.Password);
                if (result.HttpCode == StatusCodes.Status200OK)
                {
                    return Ok(result);
                }
                else
                {
                    return Unauthorized(result);
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

        [HttpPost("register")]
        public async Task<IActionResult> RegisterAccount(RegisterModel model)
        {
            try
            {
                var status = await _userService.RegisterAsync(model);
                var resp = new ResponseModels()
                {
                    HttpCode = StatusCodes.Status200OK,
                    Message = "Create user success"
                };
                return Ok(resp);
            }
            catch (Exception ex)
            {
                var resp = new ResponseModels()
                {
                    HttpCode = StatusCodes.Status400BadRequest,
                    Message = ex.Message.ToString()
                };
                return BadRequest(resp);
            }
        }

        [HttpPost("send-otp")]
        public async Task<IActionResult> SendOTP([FromBody] string email)
        {
            try
            {
                var result = await _userService.SendOTP(email);
                var resp = new ResponseModels()
                {
                    HttpCode = StatusCodes.Status200OK,
                    Message = "Gửi OTP đến tài khoản thành công",
                };
                return Ok(resp);
            }
            catch (Exception ex)
            {
                var resp = new ResponseModels()
                {
                    HttpCode = StatusCodes.Status400BadRequest,
                    Message = ex.Message.ToString()
                };
                return BadRequest(resp);
            }
        }

        [HttpPost("verify-otp")]
        public async Task<IActionResult> VerifyOTP(OtpRequestModel model)
        {
            try
            {
                //await _otpService.VerifyOtp(model);
                var result = await _otpService.VerifyOtp(model.Email, model.OTPCode);
                if (result)
                {
                    var resp = new ResponseModels()
                    {
                        HttpCode = StatusCodes.Status200OK,
                        Message = "Xác minh thành công",
                    };
                    return Ok(resp);
                }
                return BadRequest(new ResponseModels
                {
                    HttpCode = StatusCodes.Status400BadRequest,
                    Message = "OTP không đúng"
                });
            }
            catch (Exception ex)
            {
                var resp = new ResponseModels()
                {
                    HttpCode = StatusCodes.Status400BadRequest,
                    Message = ex.Message.ToString()
                };
                return BadRequest(resp);
            }
        }

        [HttpPost("forgot-password")]
        public async Task<IActionResult> AddNewPassword(AddPasswordModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _userService.AddNewPassword(model);
                if (result)
                {
                    var resp = new ResponseModels()
                    {
                        HttpCode = StatusCodes.Status200OK,
                        Message = "Đổi mật khẩu thành công",
                    };
                    return Ok(resp);
                }
                return BadRequest(new ResponseModels()
                {
                    HttpCode = StatusCodes.Status400BadRequest,
                    Message = "Không tìm thấy tài khoản",
                });
            }
            return ValidationProblem(ModelState);
        }

        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken([FromBody] string jwtToken)
        {
            try
            {
                var result = await _userService.RefreshToken(jwtToken);
                if (result.HttpCode == StatusCodes.Status200OK)
                {
                    return Ok(result);
                }
                return Unauthorized(result);
            }
            catch (Exception ex)
            {
                var resp = new ResponseModels()
                {
                    HttpCode = StatusCodes.Status400BadRequest,
                    Message = ex.Message.ToString()
                };
                return BadRequest(resp);
            }
        }

        [HttpPost("confirm-email")]
        public async Task<IActionResult> ConfirmEmail(string token)
        {
            try
            {
                var result = await _userService.ConfirmEmailAsync(token);
                if (result)
                {
                    return Ok(new ResponseModels
                    {
                        HttpCode = StatusCodes.Status200OK,
                        Message = "Xác minh tài khoản thành công"
                    });
                }
                return NotFound(new ResponseModels
                {
                    HttpCode= StatusCodes.Status404NotFound,
                    Message = "Không tìm thấy tài khoản"
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseModels
                {
                    HttpCode= StatusCodes.Status400BadRequest,
                    Message = ex.Message.ToString()
                });
            }
        }

        [HttpPost("login-with-google")]
        public async Task<IActionResult> LoginWithGoogle([FromBody] string credential)
        {
            try
            {
                var result = await _userService.LoginWithGoogle(credential);
                if (result.HttpCode == StatusCodes.Status200OK)
                {
                    return Ok(result);
                }
                return Unauthorized(result);
            }
            catch(Exception ex)
            {
                var resp = new ResponseModels()
                {
                    HttpCode = StatusCodes.Status400BadRequest,
                    Message = ex.Message.ToString()
                };
                return BadRequest(resp);
            }
        }
    }
}
