using NET1806_LittleJoy.Repository.Commons;
using NET1806_LittleJoy.Repository.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NET1806_LittleJoy.Repository.Repositories.Interface
{
    public interface IBlogRepository
    {
        public Task<Pagination<Post>> GetListBlogAsync(PaginationParameter paginationParameter);
        public Task<Post> CreateNewBlogAsync(Post blog);
        public Task<Post> UpdateBlogAsync(Post blog);
        public Task DeleteBlogAsync(Post blog);
        public Task<Post?> GetBlogByIdAsync(int id);
        public Task<Pagination<Post>> GetListBlogFilterAsync(PaginationParameter paging, BlogFilterModel filter, List<UserJoinPost> join);

        public Task<ICollection<Post>> GetListPostsAsync();
        public Task<List<Post>> GetTopBlog();
    }
}
