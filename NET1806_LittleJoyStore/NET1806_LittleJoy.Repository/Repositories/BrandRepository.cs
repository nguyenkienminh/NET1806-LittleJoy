using Microsoft.EntityFrameworkCore;
using NET1806_LittleJoy.Repository.Entities;
using NET1806_LittleJoy.Repository.Repositories.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NET1806_LittleJoy.Repository.Repositories
{
    public class BrandRepository : IBrandRepository
    {
        private readonly DbContext _context;

        public BrandRepository(LittleJoyContext context)
        {
            _context = context;
        }

        public async Task<Brand> AddBrandAsync(Brand brand)
        {
            _context.Add(brand);
            await _context.SaveChangesAsync();

            return brand;
        }
    }
}
