using NET1806_LittleJoy.Repository.Commons;
using NET1806_LittleJoy.Repository.Entities;
using NET1806_LittleJoy.Service.BusinessModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NET1806_LittleJoy.Service.Services.Interface
{
    public interface IAddressService
    {
        public Task<Pagination<AddressModel>> GetAllPagingAddressAsync(PaginationParameter paging);

        public Task<AddressModel?> GetAddressByIdAsync(int id);

        public Task<bool?> AddAddressAsync(AddressModel model);

        public Task<bool> DeleteAddressByIdAsync(int id);

        public Task<AddressModel> UpdateAddressAsync(AddressModel model);

        public Task<Pagination<AddressModel>> GetAddressListPagingByUserIdAsync(PaginationParameter paging, int id);

        public Task<AddressModel> GetMainAddressByUserIdAsync(int userId);


    }
}
