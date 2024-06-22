using Microsoft.EntityFrameworkCore.Storage;
using NET1806_LittleJoy.Repository.Commons;
using NET1806_LittleJoy.Repository.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NET1806_LittleJoy.Repository.Repositories.Interface
{
    public interface IUserRepository
    {
        Task<User> AddNewUserAsync(User user);
        Task<User> UpdateUserAsync(User user);
        public Task<User?> GetUserByUserNameAsync(string userName);
        public Task<User?> GetUserByEmailAsync(string email);
        Task<IDbContextTransaction> BeginTransactionAsync();

        /***************************************************/

        public Task<Pagination<User>> GetAllPagingUserByRoleIdAndStatusAsync(PaginationParameter paging, int roleId, bool status);

        public Task<User?> GetUserByIdAsync(int id);

        public Task<bool> DeleteUserAsync(User user);

        public Task<User> UpdateUserAsync(User userModify, User userPlace);

        public Task<ICollection<User>> GetUserListHighestScoreAsync(Role role);

        public Task<User?> GetUserByConfirmToken(string token);

        public Task<ICollection<User>> GetListUserAsync();
    }
}
