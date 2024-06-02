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
        Task<User> UpdateUser(User user);
        public Task<User?> GetUserByUserNameAsync(string userName);
        public Task<User?> GetUserByEmailAsync(string email);
        Task<IDbContextTransaction> BeginTransactionAsync();
    }
}
