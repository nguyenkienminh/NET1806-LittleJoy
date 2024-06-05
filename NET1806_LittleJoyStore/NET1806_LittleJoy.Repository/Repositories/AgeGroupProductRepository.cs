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
    public  class AgeGroupProductRepository: IAgeGroupProductRepository
    {
        private readonly LittleJoyContext _context;

        public AgeGroupProductRepository(LittleJoyContext context)
        {
            _context = context;
        }

        public async Task<Pagination<AgeGroupProduct>> GetAllAgeGroupPagingAsync(PaginationParameter paginationParameter)
        {

            var itemCount = await _context.AgeGroupProducts.CountAsync();

            var item = await _context.AgeGroupProducts
                                              .Skip((paginationParameter.PageIndex - 1) * paginationParameter.PageSize)
                                              .Take(paginationParameter.PageSize)
                                              .AsNoTracking()
                                              .ToListAsync();

            var result = new Pagination<AgeGroupProduct>(item, itemCount, paginationParameter.PageIndex, paginationParameter.PageSize);

            return result;
        }

        public async Task<AgeGroupProduct?> GetAgeGroupByIdAsync(int ageId)
        {
            var item = await _context.AgeGroupProducts
                .SingleOrDefaultAsync(p => p.Id == ageId);

            return item;
        }

        public async Task<AgeGroupProduct> AddAgeGroupAsync(AgeGroupProduct ageGroup)
        {
            _context.AgeGroupProducts.Add(ageGroup);

            await _context.SaveChangesAsync();

            return ageGroup;

        }

        public async Task<bool> RemoveAgeGroupAsync(AgeGroupProduct ageGroup)
        {
            _context.AgeGroupProducts.Remove(ageGroup);

            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<ICollection<Product>> GetProductsByAgeIdAsync(int ageId)
        {
            return await _context.Products.Where(a => a.AgeId == ageId).ToListAsync();
        }

        public async Task<AgeGroupProduct> UpdateAgeGroupAsync(AgeGroupProduct ageModify, AgeGroupProduct agePlace)
        {
            agePlace.Id = ageModify.Id;
            agePlace.AgeRange = ageModify.AgeRange;

            await _context.SaveChangesAsync();
            return agePlace;
        }
    }
}
