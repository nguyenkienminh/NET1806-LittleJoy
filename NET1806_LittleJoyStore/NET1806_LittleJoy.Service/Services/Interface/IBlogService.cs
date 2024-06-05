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
        public Task<BlogModel> CreateNewBlog(BlogModel model);
        public Task<BlogModel> GetBlogById(int Id);
        public Task<BlogModel> UpdateBlog(BlogModel blog);
        public Task<bool> RemoveBlog(int id);
    }
}
