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
    public class OriginRepository : IOriginRepository
    {
        private readonly LittleJoyContext _context;

        public OriginRepository(LittleJoyContext context)
        {
            _context = context;
        }

        public async Task<Pagination<Origin>> GetAllOriginPagingAsync(PaginationParameter paginationParameter)
        {
            var itemCount = await _context.Origins.CountAsync();

            var item = await _context.Origins.Skip((paginationParameter.PageIndex - 1) * paginationParameter.PageSize)
                                            .Take(paginationParameter.PageSize)
                                            .AsNoTracking()
                                            .ToListAsync();

            var result = new Pagination<Origin>(item, itemCount, paginationParameter.PageIndex, paginationParameter.PageSize);

            return result;
        }

        public async Task<Origin?> GetOriginByIdAsync(int originId)
        {
            return await _context.Origins.SingleOrDefaultAsync(o => o.Id == originId);
        }

        public async Task<Origin> AddOriginAsync(Origin origin)
        {
                _context.Origins.Add(origin);

                await _context.SaveChangesAsync();

            return origin;
        }

        public async Task<bool> RemoveOriginAsync(Origin origin)
        {
            _context.Remove(origin);

            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<ICollection<Product>> GetProductsByOriginIdAsync(int originId)
        {
            return await _context.Products.Where(p => p.OriginId == originId).ToListAsync();
        }

        public async Task<Origin> UpdateOriginAsync(Origin originModify, Origin originPlace)
        {
            originPlace.Id = originModify.Id;
            originPlace.OriginName = originModify.OriginName;

            await _context.SaveChangesAsync();

            return originPlace;
        }
    }
}
