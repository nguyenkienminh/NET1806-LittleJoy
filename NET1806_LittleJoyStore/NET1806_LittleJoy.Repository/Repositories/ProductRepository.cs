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
    public class ProductRepository : IProductRepositoty
    {
        private readonly LittleJoyContext _context;

        public ProductRepository(LittleJoyContext context)
        {
            _context = context;
        }

        public async Task<Pagination<Product>> GetAllProductPagingAsync(PaginationParameter paginationParameter)
        {
            
            var itemCount = await _context.Products.CountAsync();

            var item = await _context.Products.Skip((paginationParameter.PageIndex - 1) * paginationParameter.PageSize)
                                    .Take(paginationParameter.PageSize)
                                    .AsNoTracking()
                                    .ToListAsync();

            var result = new Pagination<Product>(item, itemCount, paginationParameter.PageIndex, paginationParameter.PageSize);

            return result;
        }
    }
}
