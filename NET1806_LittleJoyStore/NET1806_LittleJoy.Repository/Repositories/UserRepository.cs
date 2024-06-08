using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using NET1806_LittleJoy.Repository.Commons;
using NET1806_LittleJoy.Repository.Entities;
using NET1806_LittleJoy.Repository.Repositories.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NET1806_LittleJoy.Repository.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly LittleJoyContext _context;

        public UserRepository(LittleJoyContext context)
        {
            _context = context;
        }
        public async Task<User> AddNewUserAsync(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<IDbContextTransaction> BeginTransactionAsync()
        {
            return await _context.Database.BeginTransactionAsync();
        }

        public async Task<User?> GetUserByEmailAsync(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(x => x.Email == email);
        }

        public async Task<User?> GetUserByUserNameAsync(string userName)
        {
            return await _context.Users.FirstOrDefaultAsync(x => x.UserName == userName);
        }

        public async Task<User> UpdateUserAsync(User user)
        {
             _context.Users.Update(user);
            await _context.SaveChangesAsync();
            return user;
        }

        /*****************************************************************/

        public async Task<Pagination<User>> GetAllPagingUserByRoleIdAsync(PaginationParameter paging, int roleId)
        {
            var itemCount = await _context.Users.CountAsync(u => u.RoleId == roleId);

            var item = await _context.Users .Where(u => u.RoleId == roleId)
                                            .Skip((paging.PageIndex - 1) * paging.PageSize)
                                            .Take(paging.PageSize)
                                            .AsNoTracking()
                                            .ToListAsync();

            var result = new Pagination<User>(item, itemCount, paging.PageIndex, paging.PageSize);

            return result;
        }

        public async Task<User?> GetUserByIdAsync(int id)
        {
            return await _context.Users.SingleOrDefaultAsync(u => u.Id == id);
        }
    }
}
