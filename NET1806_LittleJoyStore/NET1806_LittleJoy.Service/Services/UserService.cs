using AutoMapper;
using Microsoft.Extensions.Configuration;
using NET1806_LittleJoy.Repository.Entities;
using NET1806_LittleJoy.Repository.Repositories.Interface;
using NET1806_LittleJoy.Service.BusinessModels;
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

        public UserService(IUserRepository userRepository, IRoleRepository roleRepository, IConfiguration configuration, IMapper mapper)
        {
            _userRepository = userRepository;
            _roleRepository = roleRepository;
            _configuration = configuration;
            _mapper = mapper;
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

        public Task<AuthenModel> RefreshToken(string jwtToken)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> RegisterAsync(RegisterModel model)
        {
            using (var transaction = await _userRepository.BeginTransactionAsync())
            {
                try
                {
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

                    await _userRepository.AddNewUserAsync(newUser);

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
    }
}
