using NET1806_LittleJoy.Repository.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NET1806_LittleJoy.Repository.Repositories.Interface
{
    public interface IRoleRepository
    {
        Task<Role> GetRoleByNameAsync(string roleName);
        Task<Role> GetRoleByIdAsync(int id);

        public Task<List<Role>> GetAllRoleAsync();

        public Task<bool?> AddRoleAsync(Role role);

        public Task<bool> RemoveRoleByIdAsync(Role role);

        public Task<ICollection<User>> GetUserByRoleIdAsync(int roleId);

        public Task<Role> UpdateRoleAsync(Role roleModify, Role rolePlace);
    }   
}
