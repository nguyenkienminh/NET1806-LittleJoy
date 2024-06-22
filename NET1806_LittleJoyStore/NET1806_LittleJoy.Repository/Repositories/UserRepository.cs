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

        public async Task<Pagination<User>> GetAllPagingUserByRoleIdAndStatusAsync(PaginationParameter paging, int roleId, bool status)
        {
            var itemCount = await _context.Users.CountAsync(u => u.RoleId == roleId && u.Status == status);

            var item = await _context.Users .Where(u => u.RoleId == roleId && u.Status == status)
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

        public async Task<bool> DeleteUserAsync(User user)
        {
            user.Status = false;

            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<User> UpdateUserAsync(User userModify, User userPlace)
        {
            userPlace.Fullname = userModify.Fullname;
            userPlace.PhoneNumber = userModify.PhoneNumber;
            userPlace.Status = userModify.Status;
            userPlace.Avatar = userModify.Avatar;
            userPlace.UnsignName = userModify.UnsignName;
            userPlace.RoleId = userModify.RoleId;

            await _context.SaveChangesAsync();

            return userPlace;
        }

        public async Task<ICollection<User>> GetUserListHighestScoreAsync(Role role)
        {

            var result = await _context.Users.Where(u => u.RoleId == role.Id && u.Status == true).OrderByDescending(u => u.Points).Take(5).ToListAsync();

            return result;
        }

        public async Task<User?> GetUserByConfirmToken(string token)
        {
            return await _context.Users.SingleOrDefaultAsync(x => x.TokenConfirmEmail == token);
        }

        public async Task<ICollection<User>> GetListUserAsync()
        {
            return await _context.Users.ToListAsync();
        }
    }
}
