using AutoMapper;
using Google.Apis.Auth;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using NET1806_LittleJoy.Repository.Commons;
using NET1806_LittleJoy.Repository.Entities;
using NET1806_LittleJoy.Repository.Repositories.Interface;
using NET1806_LittleJoy.Service.BusinessModels;
using NET1806_LittleJoy.Service.Enums;
using NET1806_LittleJoy.Service.Helpers;
using NET1806_LittleJoy.Service.Services.Interface;
using NET1806_LittleJoy.Service.Ultils;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace NET1806_LittleJoy.Service.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;
        private readonly IMailService _mailService;
        private readonly IOtpService _otpService;
        private readonly IAddressService _address;

        public UserService(IUserRepository userRepository, IRoleRepository roleRepository, IMailService mailService, IOtpService otpService, IConfiguration configuration, IAddressService address, IMapper mapper)
        {
            _userRepository = userRepository;
            _roleRepository = roleRepository;
            _configuration = configuration;
            _mapper = mapper;
            _mailService = mailService;
            _otpService = otpService;
            _address = address;
        }

        public async Task<bool> AddNewPassword(AddPasswordModel model)
        {
            var user = await _userRepository.GetUserByEmailAsync(model.Email);
            if (user == null)
            {
                return false;
            }
            var hashPassword = PasswordUtils.HashPassword(model.Password);
            user.PasswordHash = hashPassword;
            await _userRepository.UpdateUserAsync(user);
            return true;
        }

        public async Task<AuthenModel> LoginByUsernameAndPassword(string username, string password)
        {
            var user = await _userRepository.GetUserByUserNameAsync(username);
            if (user == null)
            {
                return new AuthenModel()
                {
                    HttpCode = 401,
                    Message = "Account does not exist"
                };
            }
            else
            {
                var checkPassword = PasswordUtils.VerifyPassword(password, user.PasswordHash);
                if (checkPassword)
                {
                    var accessToken = await GenerateAccessToken(username, user);
                    var refeshToken = GenerateRefreshToken(username);

                    return new AuthenModel()
                    {
                        AccessToken = accessToken,
                        RefreshToken = refeshToken,
                        HttpCode = 200,
                    };
                }
                else
                {
                    return new AuthenModel()
                    {
                        HttpCode = 400,
                        Message = "Wrong Password"
                    };
                }
            }
        }

        public async Task<AuthenModel> RefreshToken(string jwtToken)
        {
            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:SecretKey"]));
            var handler = new JwtSecurityTokenHandler();
            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = authSigningKey,
                ValidateIssuer = true,
                ValidIssuer = _configuration["JWT:ValidIssuer"],
                ValidateAudience = true,
                ValidAudience = _configuration["JWT:ValidAudience"],
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            };
            try
            {
                SecurityToken validatedToken;
                var principal = handler.ValidateToken(jwtToken, validationParameters, out validatedToken);
                var userName = principal.Claims.FirstOrDefault(x => x.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name").Value;
                if (userName != null)
                {
                    var existUser = await _userRepository.GetUserByUserNameAsync(userName);
                    if (existUser != null)
                    {
                        var accessToken = await GenerateAccessToken(userName, existUser);
                        var refreshToken = GenerateRefreshToken(userName);
                        return new AuthenModel
                        {
                            HttpCode = 200,
                            Message = "Refresh token successfully",
                            AccessToken = accessToken,
                            RefreshToken = refreshToken
                        };
                    }
                }
                return new AuthenModel
                {
                    HttpCode = 401,
                    Message = "User does not exits."
                };
            }
            catch (Exception ex)
            {
                throw new Exception("Token is not valid.");
            }
        }

        public async Task<bool> RegisterAsync(RegisterModel model)
        {
            using (var transaction = await _userRepository.BeginTransactionAsync())
            {
                try
                {
                    if (model.UserName.StartsWith("GID_", StringComparison.OrdinalIgnoreCase))
                    {
                        throw new Exception("Username không được bắt đầu bằng GID_");
                    }
                    User newUser = new User()
                    {
                        UserName = model.UserName,
                        Fullname = model.FullName,
                        Email = model.Email,
                        PhoneNumber = model.PhoneNumber,
                        UnsignName = StringUtils.ConvertToUnSign(model.FullName)
                    };

                    var checkEmail = await _userRepository.GetUserByEmailAsync(newUser.Email);
                    var checkUsername = await _userRepository.GetUserByUserNameAsync(newUser.UserName);
                    if (checkEmail != null)
                    {
                        throw new Exception("Account already exists.");
                    }
                    else if (checkUsername != null)
                    {
                        throw new Exception("Username already exists.");
                    }

                    newUser.PasswordHash = PasswordUtils.HashPassword(model.Password);
                    var role = await _roleRepository.GetRoleByNameAsync("USER");
                    newUser.RoleId = role.Id;

                    newUser.Status = true;
                    newUser.ConfirmEmail = false;
                    newUser.Points = 0;

                    newUser.TokenConfirmEmail = Guid.NewGuid().ToString();

                    await _userRepository.AddNewUserAsync(newUser);

                    await _mailService.sendEmailAsync(new MailRequest()
                    {
                        ToEmail = newUser.Email,
                        Body = EmailContent.ConfirmEmail(newUser.UserName, newUser.TokenConfirmEmail),
                        Subject = "[LittleJoy] Confirm Email"
                    });

                    await transaction.CommitAsync();
                    return true;
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    throw;
                }
            }
        }

        public async Task<bool> SendOTP(string email)
        {
            var checkUser = await _userRepository.GetUserByEmailAsync(email);

            if (checkUser == null)
            {
                throw new Exception("Account does not exist");
            }

            if (checkUser.GoogleId != null)
            {
                throw new Exception("Account cannot reset password");
            }

            var otp = await _otpService.AddNewOtp(email);

            var requestMail = new MailRequest()
            {
                ToEmail = email,
                Subject = "[LittleJoy] Password reset",
                Body = EmailContent.EmailOTPContent(checkUser.UserName, otp.OTPCode)
            };
            await _mailService.sendEmailAsync(requestMail);
            await _userRepository.UpdateUserAsync(checkUser);
            return true;
        }

        private async Task<string> GenerateAccessToken(string username, User user)
        {
            var role = await _roleRepository.GetRoleByIdAsync(user.RoleId.Value);

            var authClaims = new List<Claim>();

            if (role != null)
            {
                authClaims.Add(new Claim(ClaimTypes.Name, user.UserName));
                authClaims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));
                authClaims.Add(new Claim(ClaimTypes.Role, role.RoleName));
                //authClaims.Add(new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()));
                authClaims.Add(new Claim("user_ID", user.Id.ToString()));
            }
            var accessToken = GenerateJWTToken.CreateToken(authClaims, _configuration, DateTime.UtcNow);
            return new JwtSecurityTokenHandler().WriteToken(accessToken);
        }

        private string GenerateRefreshToken(string username)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, username),
            };
            var refreshToken = GenerateJWTToken.CreateRefreshToken(claims, _configuration, DateTime.UtcNow);
            return new JwtSecurityTokenHandler().WriteToken(refreshToken).ToString();
        }


        /**************************************************************/

        public async Task<Pagination<UserModel>> GetAllPagingUserByRoleIdAndStatusAsync(PaginationParameter paging, int roleId, bool status)
        {

            var listUser = await _userRepository.GetAllPagingUserByRoleIdAndStatusAsync(paging, roleId, status);

            if (!listUser.Any())
            {
                return null;
            }

            var listUserModel = listUser.Select(a => new UserModel
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


            return new Pagination<UserModel>(listUserModel,
                listUser.TotalCount,
                listUser.CurrentPage,
                listUser.PageSize);
        }

        public async Task<UserModel?> GetUserByIdAsync(int id)
        {
            var userDetail = await _userRepository.GetUserByIdAsync(id);

            if (userDetail == null)
            {
                return null;
            }

            var userDetailModel = _mapper.Map<UserModel>(userDetail);

            return userDetailModel;
        }

        public async Task<bool?> AddUserAsync(UserModel model, string mainAddress)
        {
            try
            {

                if (model.PhoneNumber != null)
                {
                    if (StringUtils.IsValidPhoneNumber(model.PhoneNumber) == false)
                    {
                        return false;
                    }
                }

                if (model.Email != null)
                {
                    if (StringUtils.IsValidEmail(model.Email) == false)
                    {
                        return false;
                    }
                }

                var userInfo = _mapper.Map<User>(model);

                if (model.Password != null)
                {
                    userInfo.PasswordHash = PasswordUtils.HashPassword(model.Password);
                }
                else
                {
                    return null;
                }

                userInfo.Status = true;
                userInfo.ConfirmEmail = false;
                userInfo.Points = 0;

                if (userInfo.Fullname != null)
                {
                    userInfo.UnsignName = StringUtils.ConvertToUnSign(userInfo.Fullname);
                }

                var userAdded = await _userRepository.AddNewUserAsync(userInfo);


                if (!mainAddress.Equals(""))
                {
                    await _address.AddAddressAsync(new AddressModel()
                    {
                        Address1 = mainAddress,
                        UserId = userAdded.Id,
                    });
                }

                return true;
            }
            catch (Exception ex)
            {

                return false;
            }
        }

        public async Task<bool> DeleteUserByIdAsync(int id)
        {
            var removeUser = await _userRepository.GetUserByIdAsync(id);

            if (removeUser == null)
            {
                return false;
            }

            return await _userRepository.DeleteUserAsync(removeUser);
        }

        public async Task<UserModel> UpdateUserAsync(UserModel model, string mainAddress)
        {
            var userModify = _mapper.Map<User>(model);

            var userPlace = await _userRepository.GetUserByIdAsync(userModify.Id);

            if (userPlace == null)
            {
                return null;
            }

            if (userModify.Fullname != "".Trim() && userModify.Fullname != null)
            {
                userModify.UnsignName = StringUtils.ConvertToUnSign(userModify.Fullname);
            }
            else
            {
                userModify.Fullname = userPlace.Fullname;
                userModify.UnsignName = StringUtils.ConvertToUnSign(userPlace.Fullname);
            }

            if (userModify.PhoneNumber != null && userModify.PhoneNumber != "".Trim())
            {
                if (StringUtils.IsValidPhoneNumber(userModify.PhoneNumber) == false)
                {
                    return null;
                }
            }
            else
            {
                userModify.PhoneNumber = userPlace.PhoneNumber;
            }

            if (userModify.Status == null)
            {
                userModify.Status = userPlace.Status;
            }

            userModify.Avatar = userPlace.Avatar;


            var updateUser = await _userRepository.UpdateUserAsync(userModify, userPlace);

            if (updateUser != null)
            {

                if (!mainAddress.Equals(""))
                {
                    var addressUserMain = await _address.GetMainAddressByUserIdAsync(updateUser.Id);

                    if (addressUserMain == null)
                    {
                        await _address.AddAddressAsync(new AddressModel()
                        {
                            Address1 = mainAddress,
                            UserId = updateUser.Id,
                        });

                    }
                    else
                    {
                        if (!mainAddress.Equals(addressUserMain.Address1))
                        {
                            var addressResponse = await _address.UpdateAddressAsync(new AddressModel()
                            {
                                Id = addressUserMain.Id,
                                Address1 = mainAddress,
                                IsMainAddress = true
                            });

                            if (addressResponse == null)
                            {
                                return null;
                            }
                        }
                    }
                }


                return _mapper.Map<UserModel>(updateUser);
            }
            return null;
        }

        public async Task<UserModel> UpdateUserRoleAsync(UserModel model)
        {
            var userModify = _mapper.Map<User>(model);

            var userPlace = await _userRepository.GetUserByIdAsync(userModify.Id);

            if (userPlace == null)
            {
                return null;
            }

            if (userModify.Fullname != "".Trim() && userModify.Fullname != null)
            {
                userModify.UnsignName = StringUtils.ConvertToUnSign(userModify.Fullname);
            }
            else
            {
                userModify.Fullname = userPlace.Fullname;
                userModify.UnsignName = StringUtils.ConvertToUnSign(userPlace.Fullname);
            }

            if (userModify.PhoneNumber != null && userModify.PhoneNumber != "".Trim())
            {
                if (StringUtils.IsValidPhoneNumber(userModify.PhoneNumber) == false)
                {
                    return null;
                }
            }
            else
            {
                userModify.PhoneNumber = userPlace.PhoneNumber;
            }

            if (userModify.Avatar == null || userModify.Avatar == "".Trim())
            {
                userModify.Avatar = userPlace.Avatar;
            }

            userModify.Status = userPlace.Status;

            var updateUser = await _userRepository.UpdateUserAsync(userModify, userPlace);

            if (updateUser != null)
            {
                return _mapper.Map<UserModel>(updateUser);
            }
            return null;
        }

        public async Task<string> ChangePasswordUserRoleAsync(ChangePasswordModel model)
        {

            var user = await _userRepository.GetUserByIdAsync(model.Id);

            if (user == null)
            {
                return null;
            }

            var checkPassword = PasswordUtils.VerifyPassword(model.OldPassword, user.PasswordHash);

            if (checkPassword)
            {
                AddPasswordModel addPassword = new AddPasswordModel()
                {
                    Email = user.Email,
                    Password = model.NewPassword,
                    ConfirmPassword = model.ConfirmPassword,
                };

                var result = await AddNewPassword(addPassword);

                if (result == true)
                {
                    return "Add Password Success";
                }
                else
                {
                    return "Add Failed";
                }
            }

            return "Password Incorrect";

        }

        public async Task<ICollection<UserModel>> GetUserListHighestScoreAsync()
        {

            var roleUser = await _roleRepository.GetRoleByNameAsync("USER");

            var list = await _userRepository.GetUserListHighestScoreAsync(roleUser);

            var listUserModel = list.Select(a => new UserModel
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

            return listUserModel;
        }

        public async Task<bool> ConfirmEmailAsync(string token)
        {
            var user = await _userRepository.GetUserByConfirmToken(token);
            if(user == null)
            {
                return false;
            }
            user.ConfirmEmail = true;
            user.TokenConfirmEmail = "";
            await _userRepository.UpdateUserAsync(user);
            await _mailService.sendEmailAsync(new MailRequest
            {
                ToEmail = user.Email,
                Subject = "[LittleJoy] Welcome to Little Joy Store",
                Body = EmailContent.WelcomeEmail(user.UserName)
            });
            return true;
        }

        public async Task<AuthenModel> LoginWithGoogle(string credental)
        {
            string cliendId = _configuration["GoogleCredential:ClientId"];
            var settings = new GoogleJsonWebSignature.ValidationSettings()
            {
                Audience = new List<string> { cliendId }
            };
            var payload = await GoogleJsonWebSignature.ValidateAsync(credental, settings);

            if(payload == null)
            {
                throw new Exception("Credental không hợp lệ");
            }

            var existUser = await _userRepository.GetUserByEmailAsync(payload.Email);

            //nếu có user
            if(existUser != null)
            {
                if(existUser.Status == false)
                {
                    throw new Exception("Tài khoản đã bị cấm");
                }
                var accessToken = await GenerateAccessToken(existUser.Email, existUser);
                var refreshToken = GenerateRefreshToken(existUser.Email);

                return new AuthenModel()
                {
                    HttpCode = 200,
                    Message = "Login with Google sucessfully",
                    AccessToken = accessToken,
                    RefreshToken = refreshToken
                };
            } else
            {
                //create new user
                using (var transaction = await _userRepository.BeginTransactionAsync())
                {
                    try
                    {
                        User user = new User()
                        {
                            Email = payload.Email,
                            Avatar = payload.Picture,
                            Fullname = payload.Name,
                            UserName = "GID_" + payload.Email.Split("@")[0],
                            Status = true,
                            ConfirmEmail = true,
                            RoleId = 1,
                            GoogleId = payload.JwtId,
                            Points = 0,
                            UnsignName = StringUtils.ConvertToUnSign(payload.Name)
                        };
                        await _userRepository.AddNewUserAsync(user);
                        await transaction.CommitAsync();

                        var accessToken = await GenerateAccessToken(user.Email, user);
                        var refreshToken = GenerateRefreshToken(user.Email);

                        await _mailService.sendEmailAsync(new MailRequest
                        {
                            ToEmail = user.Email,
                            Subject = "[LittleJoy] Welcome to Little Joy Store",
                            Body = EmailContent.WelcomeEmail(user.UserName)
                        });

                        return new AuthenModel()
                        {
                            HttpCode = 200,
                            Message = "Login with Google sucessfully",
                            AccessToken = accessToken,
                            RefreshToken = refreshToken
                        };
                    }
                    catch
                    {
                        await transaction.RollbackAsync();
                        throw;
                    }
                }
            }
        }
    }
}
