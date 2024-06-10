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
        public Task<Pagination<UserModel>> GetAllPagingUserByRoleIdAsync(PaginationParameter paging, int roleId);

        public Task<UserModel?> GetUserByIdAsync(int id);

        public Task<bool?> AddUserAsync(UserModel model);

        public Task<bool> DeleteUserByIdAsync(int id);

        public Task<UserModel> UpdateUserAsync(UserModel model);

        public Task<UserModel> UpdateUserRoleAsync(UserModel model);

        public Task<string> ChangePasswordUserRoleAsync(ChangePasswordModel model);

        public Task<ICollection<UserModel>> GetUserListHighestScoreAsync();

    }
}
