using Microsoft.EntityFrameworkCore;
using NET1806_LittleJoy.Repository.Commons;
using NET1806_LittleJoy.Repository.Entities;
using NET1806_LittleJoy.Repository.Repositories.Interface;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NET1806_LittleJoy.Repository.Repositories
{
    public class BrandRepository : IBrandRepository
    {
        private readonly LittleJoyContext _context;

        public BrandRepository(LittleJoyContext context)
        {
            _context = context;
        }

        public async Task<Pagination<Brand>> GetAllBrandPagingAsync(PaginationParameter paginationParameter)
        {
            var itemCount = await _context.Brands.CountAsync();

            var item = await _context.Brands.Skip((paginationParameter.PageIndex - 1) * paginationParameter.PageSize)
                                            .Take(paginationParameter.PageSize)
                                            .AsNoTracking()
                                            .ToListAsync();

            var result = new Pagination<Brand>(item, itemCount, paginationParameter.PageIndex, paginationParameter.PageSize);

            return result;
        }

        public async Task<Brand?> GetBrandByIdAsync(int brandId)
        {
            return await _context.Brands.SingleOrDefaultAsync(b => b.Id == brandId);
        }

        public async Task<Brand> AddBrandAsync(Brand brand)
        {
            _context.Brands.Add(brand);

            await _context.SaveChangesAsync();

            return brand;
        }

        public async Task<bool> RemoveBrandAsync(Brand brand)
        {

            //check xem danh sach co product nao co brand do
            // neu co thi ko dc xoa
            // neu ko thi xoa

            _context.Brands.Remove(brand);

            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<ICollection<Product>> GetProductsByBrandIdAsync(int id)
        {
            return await _context.Products.Where(pb => pb.BrandId == id).ToListAsync();
        }

        public async Task<Brand> UpdateBrandAsync(Brand brandModify, Brand brandPlace)
        {
            brandPlace.Id = brandModify.Id;
            brandPlace.BrandName = brandModify.BrandName;
            brandPlace.BrandDescription = brandModify.BrandDescription;
            brandPlace.Logo = brandModify.Logo;

            await _context.SaveChangesAsync();
            return brandPlace;
        }
    }
}
