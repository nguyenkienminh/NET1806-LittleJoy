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
    }
}
