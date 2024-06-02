using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NET1806_LittleJoy.API.ViewModels.RequestModels;
using NET1806_LittleJoy.API.ViewModels.ResponeModels;
using NET1806_LittleJoy.Service.BusinessModels;
using NET1806_LittleJoy.Service.Services.Interface;

namespace NET1806_LittleJoy.API.Controllers
{
    [Route("api/[controller]")]
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

        [HttpPost("Login")]
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

        [HttpPost("Register")]
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

        [HttpPost("SendOTP")]
        public async Task<IActionResult> SendOTP(string email)
        {
            try
            {
                var result = await _userService.SendOTP(email);
                var resp = new ResponseModels()
                {
                    HttpCode = StatusCodes.Status200OK,
                    Message = "Send OTP Successfully",
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

        [HttpPost("VerifyOTP")]
        public async Task<IActionResult> VerifyOTP(string email, int OTPCode)
        {
            try
            {
                await _otpService.VerifyOtp(email, OTPCode);
                var resp = new ResponseModels()
                {
                    HttpCode = StatusCodes.Status200OK,
                    Message = "Xác minh thành công",
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

        [HttpPost("Refresh-Token")]
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
    }
}
