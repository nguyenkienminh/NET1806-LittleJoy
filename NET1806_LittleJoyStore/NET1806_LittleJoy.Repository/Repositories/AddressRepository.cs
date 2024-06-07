using Microsoft.EntityFrameworkCore;
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
    public class AddressRepository : IAddressRepository
    {
        private readonly LittleJoyContext _context;

        public AddressRepository(LittleJoyContext context)
        {
            _context = context;
        }

        public async Task<Pagination<Address>> GetAllPagingAddressAsync(PaginationParameter paging)
        {
            var itemCount = await  _context.Addresses.CountAsync();

            var item = await _context.Addresses.Skip((paging.PageIndex - 1) * paging.PageSize)
                                            .Take(paging.PageSize)
                                            .AsNoTracking()
                                            .ToListAsync();

            var result = new Pagination<Address>(item, itemCount, paging.PageIndex, paging.PageSize);

            return result;
        }


        public async Task<Address?> GetAddressByIdAsync(int id)
        {
            return await _context.Addresses.SingleOrDefaultAsync(x => x.Id == id);
        }

        public async Task<bool?> AddAddressAsync(Address address)
        {   
            _context.Addresses.Add(address);

            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<int> CountAddressAsyncByUserId(int id)
        {
            return await _context.Addresses.CountAsync(x => x.UserId == id);
        }
    }
}
