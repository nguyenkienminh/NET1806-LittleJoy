using Microsoft.EntityFrameworkCore.Storage;
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
        Task<ICollection<User>> GetAllUserAsync();
        Task<ICollection<User>> SearchUserAsync(string search);
        public Task<User?> GetUserByUserNameAsync(string userName);
        public Task<User?> GetUserByEmailAsync(string email);
        Task<int> DeleteUserAsync(User user);
        Task<IDbContextTransaction> BeginTransactionAsync();
    }
}
