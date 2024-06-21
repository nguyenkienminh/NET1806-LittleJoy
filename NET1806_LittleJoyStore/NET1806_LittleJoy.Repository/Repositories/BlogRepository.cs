using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.IdentityModel.Tokens;
using NET1806_LittleJoy.Repository.Commons;
using NET1806_LittleJoy.Repository.Entities;
using NET1806_LittleJoy.Repository.Repositories.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace NET1806_LittleJoy.Repository.Repositories
{
    public class BlogRepository : IBlogRepository
    {
        private readonly LittleJoyContext _context;

        public BlogRepository(LittleJoyContext context)
        {
            _context = context;
        }

        public async Task<Post> CreateNewBlogAsync(Post blog)
        {
            _context.Posts.Add(blog);
            await _context.SaveChangesAsync();
            return blog;
        }

        public async Task DeleteBlogAsync(Post blog)
        {
            _context.Posts.Remove(blog);
            await _context.SaveChangesAsync(true);
        }

        public async Task<Post?> GetBlogByIdAsync(int id)
        {
            return await _context.Posts.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Pagination<Post>> GetListBlogAsync(PaginationParameter paginationParameter)
        {
            var itemCount = await _context.Posts.CountAsync();
            var items = await _context.Posts.OrderByDescending(x => x.Id).Skip((paginationParameter.PageIndex - 1) * paginationParameter.PageSize)
                .Take(paginationParameter.PageSize)
                .AsNoTracking().ToListAsync();
            var result = new Pagination<Post>(items, itemCount, paginationParameter.PageIndex, paginationParameter.PageSize);

            return result;
        }

        public async Task<Pagination<Post>> GetListBlogFilterAsync(PaginationParameter paging, BlogFilterModel filter, List<UserJoinPost> joinPosts)
        {
            var query = _context.Posts.AsQueryable(); 
            var other = query;

            if (!string.IsNullOrEmpty(filter.UserName))
            {
                var filteredJoinPosts = joinPosts.Where(x => x.UserName.Contains(filter.UserName)).ToList(); 

                if (filteredJoinPosts.Any())
                {
                    var userIds = filteredJoinPosts.Select(x => x.UserId).ToList();
                    query = query.Where(x => userIds.Contains(x.UserId)); 
                }
                else
                {
                    other = query.Where(x => x.UserId == -1); //filter UserName bị sai - 2
                }
                
            }

            if(other.Any()) 
            {
                if (!string.IsNullOrEmpty(filter.search))
                {
                    query = query.Where(x => x.UnsignTitle.Contains(filter.search)); 
                }
            }

            if(other.Any())
            {
                if (filter.sortDate.HasValue)
                {
                    if (filter.sortDate == 1)
                    {
                        query = query.OrderBy(x => x.Date);
                    }
                    else
                    {
                        query = query.OrderByDescending(x => x.Date);
                    }
                }
            }

            Pagination<Post> result = null;

            if (other.Any())
            {
                var itemCount = await query.CountAsync();
                var items = await query.Skip((paging.PageIndex - 1) * paging.PageSize)
                    .Take(paging.PageSize)
                    .AsNoTracking().ToListAsync();
                result = new Pagination<Post>(items, itemCount, paging.PageIndex, paging.PageSize);
            }
            else
            {
                var itemCount = await other.CountAsync();
                var items = await other.Skip((paging.PageIndex - 1) * paging.PageSize)
                    .Take(paging.PageSize)
                    .AsNoTracking().ToListAsync();
                result = new Pagination<Post>(items, itemCount, paging.PageIndex, paging.PageSize);
            }
            
            return result;
        }

        public async Task<Post> UpdateBlogAsync(Post blog)
        {
            _context.Posts.Update(blog);
            await _context.SaveChangesAsync();
            return blog;
        }

        public async Task<List<Post>> GetTopBlog()
        {
            var list = _context.Posts.OrderByDescending(x => x.Id).Take(3);
            return await list.ToListAsync();
        }

        public async Task<ICollection<Post>> GetListPostsAsync()
        {
            return await _context.Posts.ToListAsync();
        }
    }
}
