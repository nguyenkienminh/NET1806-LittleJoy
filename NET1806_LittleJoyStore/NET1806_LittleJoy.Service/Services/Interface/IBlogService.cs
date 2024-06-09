using NET1806_LittleJoy.Repository.Commons;
using NET1806_LittleJoy.Service.BusinessModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NET1806_LittleJoy.Service.Services.Interface
{
    public interface IBlogService
    {
        public Task<Pagination<BlogModel>> GetListBlogAsync(PaginationParameter paginationParameter);
        public Task<BlogModel> CreateNewBlogAsync(BlogModel model);
        public Task<BlogModel> GetBlogByIdAsync(int Id);
        public Task<BlogModel> UpdateBlogAsync(BlogModel blog);
        public Task<bool> RemoveBlogAsync(int id);
        public Task<Pagination<BlogModel>> GetListBlogFilterAsync(PaginationParameter paging, BlogFilterModel filter);
    }
}
