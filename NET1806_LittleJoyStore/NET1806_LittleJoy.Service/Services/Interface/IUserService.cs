using NET1806_LittleJoy.Repository.Commons;
using NET1806_LittleJoy.Service.BusinessModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NET1806_LittleJoy.Service.Services.Interface
{
    public interface IUserService
    {
        public Task<AuthenModel> LoginByUsernameAndPassword(string username, string password);
        public Task<bool> RegisterAsync(RegisterModel model);
        public Task<AuthenModel> RefreshToken(string jwtToken);
        public Task<bool> SendOTP(string email);
        public Task<bool> AddNewPassword(AddPasswordModel model);

        /*******************************************************************/
        public Task<Pagination<UserModel>> GetAllPagingUserByRoleIdAndStatusAsync(PaginationParameter paging, int roleId, bool status);

        public Task<UserModel?> GetUserByIdAsync(int id);

        public Task<UserModel?> GetUserByNameAsync(string name);

        public Task<bool?> AddUserAsync(UserModel model, string mainAddress);

        public Task<bool> DeleteUserByIdAsync(int id);

        public Task<UserModel> UpdateUserAsync(UserModel model, string mainAddress);

        public Task<UserModel> UpdateUserRoleAsync(UserModel model);

        public Task<bool?> ChangePasswordUserRoleAsync(ChangePasswordModel model);

        public Task<ICollection<UserModel>> GetUserListHighestScoreAsync();

        public Task<bool> ConfirmEmailAsync(string token);

        public Task<AuthenModel> LoginWithGoogle(string credental);
    }
}
