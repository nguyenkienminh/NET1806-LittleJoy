using NET1806_LittleJoy.Service.BusinessModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NET1806_LittleJoy.Service.Services.Interface
{
    public interface IRoleService
    {
        public Task<List<RoleModel>> GetAllRoleAsync();

        public Task<RoleModel> GetRoleByIdAsync(int id);

        public Task<bool> AddRoleAsync(RoleModel model);

        public Task<bool> RemoveRoleByIdAsync(int Id);

        public Task<RoleModel> UpdateRoleAsync(RoleModel model);
    }
}
