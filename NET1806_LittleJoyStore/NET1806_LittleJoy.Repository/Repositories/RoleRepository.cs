using Microsoft.EntityFrameworkCore;
using NET1806_LittleJoy.Repository.Entities;
using NET1806_LittleJoy.Repository.Repositories.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NET1806_LittleJoy.Repository.Repositories
{
    public class RoleRepository : IRoleRepository
    {
        private readonly LittleJoyContext _context;

        public RoleRepository(LittleJoyContext context)
        {
            _context = context;
        }

        public async Task<List<Role>> GetAllRoleAsync()
        {
            return await _context.Roles.ToListAsync();
        }

        public async Task<Role> GetRoleByIdAsync(int id)
        {
            return await _context.Roles.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Role> GetRoleByNameAsync(string roleName)
        {
            return await _context.Roles.FirstOrDefaultAsync(x => x.RoleName == roleName);
        }

        public async Task<bool?> AddRoleAsync(Role role)
        {
            _context.Roles.Add(role);

            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> RemoveRoleByIdAsync(Role role)
        {
            _context.Roles.Remove(role);

            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<ICollection<User>> GetUserByRoleIdAsync(int roleId)
        {
            return await _context.Users.Where(p => p.RoleId == roleId).ToListAsync();
        }

        public async Task<Role> UpdateRoleAsync(Role roleModify, Role rolePlace)
        {
            rolePlace.Id = roleModify.Id;
            rolePlace.RoleName = roleModify.RoleName;

            await _context.SaveChangesAsync();

            return rolePlace;
        }
    }
}
