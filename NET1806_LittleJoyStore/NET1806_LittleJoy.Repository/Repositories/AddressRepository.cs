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

        public async Task<int> CountAddressByUserIdAsync(int id)
        {
            return await _context.Addresses.CountAsync(x => x.UserId == id);
        }

        public async Task<ICollection<Address>> GetAddressByUserIdAsync(int id)
        {
            return await _context.Addresses.Where(x => x.UserId == id).ToListAsync();
        }

        public async Task<bool> DeleteAddressAsync(Address address)
        {
            _context.Addresses.Remove(address);

            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<Address> UpdateAddressAsync(Address addressModify, Address addressPlace)
        {
            addressPlace.Id = addressModify.Id;
            addressPlace.Address1 = addressModify.Address1;
            addressPlace.IsMainAddress = addressModify.IsMainAddress;
            addressPlace.UserId = addressModify.UserId;
            
            await _context.SaveChangesAsync();

            return addressPlace;
        }

        public async Task<Pagination<Address>> GetAddressListPagingByUserIdAsync(PaginationParameter paging, int id)
        {
            var itemCount = await _context.Addresses.CountAsync(a => a.UserId == id);

            var item = await _context.Addresses.Where(a => a.UserId == id)
                                                .Skip((paging.PageIndex - 1) * paging.PageSize)
                                                .Take(paging.PageSize)
                                                .AsNoTracking()
                                                .ToListAsync();

            var result = new Pagination<Address>(item, itemCount, paging.PageIndex, paging.PageSize);

            return result;
        }

        public async Task<Address?> GetMainAddressByUserIdAsync(int id)
        {
            return await _context.Addresses.SingleOrDefaultAsync(x => x.IsMainAddress == true && x.UserId == id);
        }
    }
}
