using NET1806_LittleJoy.Repository.Commons;
using NET1806_LittleJoy.Repository.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NET1806_LittleJoy.Repository.Repositories.Interface
{
    public interface IAddressRepository
    {
        public Task<Pagination<Address>> GetAllPagingAddressAsync(PaginationParameter paging);

        public Task<Address?> GetAddressByIdAsync(int id);

        public Task<bool?> AddAddressAsync(Address address);

        public Task<int> CountAddressAsyncByUserId(int id);

    }
}
