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
    public class BlogRepository : IBlogRepository
    {
        private readonly LittleJoyContext _context;

        public BlogRepository(LittleJoyContext context)
        {
            _context = context;
        }

        public async Task<Pagination<Post>> GetListBlogAsync(PaginationParameter paginationParameter)
        {
            var itemCount = await _context.Posts.CountAsync();
            var items = await _context.Posts.Skip((paginationParameter.PageIndex - 1) * paginationParameter.PageSize)
                .Take(paginationParameter.PageSize)
                .AsNoTracking().ToListAsync();
            var result = new Pagination<Post>(items, itemCount, paginationParameter.PageIndex, paginationParameter.PageSize);

            return result;
        }
    }
}
