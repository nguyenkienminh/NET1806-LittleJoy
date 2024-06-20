using AutoMapper;
using NET1806_LittleJoy.Repository.Commons;
using NET1806_LittleJoy.Repository.Entities;
using NET1806_LittleJoy.Repository.Repositories;
using NET1806_LittleJoy.Repository.Repositories.Interface;
using NET1806_LittleJoy.Service.BusinessModels;
using NET1806_LittleJoy.Service.Services.Interface;
using NET1806_LittleJoy.Service.Ultils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NET1806_LittleJoy.Service.Services
{
    public class BlogService : IBlogService
    {
        private readonly IBlogRepository _blogRepository;
        private readonly IMapper _mapper;
        private readonly IUserRepository _userRepository;

        public BlogService(IBlogRepository blogRepository, IMapper mapper, IUserRepository userRepository) 
        {
            _blogRepository = blogRepository;
            _mapper = mapper;
            _userRepository = userRepository;
        }

        public async Task<BlogModel> CreateNewBlogAsync(BlogModel model)
        {
            if(model == null)
            {
                return null;
            }
            var blogModel = _mapper.Map<Post>(model);
            blogModel.UnsignTitle = StringUtils.ConvertToUnSign(blogModel.Title);
            blogModel.Date = DateTime.UtcNow.AddHours(7);
            var blog = await _blogRepository.CreateNewBlogAsync(blogModel);
            if(blog != null)
            {
                return _mapper.Map<BlogModel>(blog);
            }
            else
            {
                return null;
            }
            
        }

        public async Task<Pagination<BlogModel>> GetListBlogFilterAsync(PaginationParameter paging, BlogFilterModel filter)
        {
            #region
            var posts = await _blogRepository.GetListPostsAsync();
            var users = await _userRepository.GetListUserAsync();

            var join = posts.Join(users, p => p.UserId, u => u.Id,
                (posts, users) => new
                {
                    UserId = posts.UserId,
                    UserName = users.UserName,
                }).ToList();

            List<UserJoinPost> joinPosts = new List<UserJoinPost>
                (join.Select(x => new UserJoinPost
            {
                    UserId = x.UserId,
                    UserName = x.UserName,
            })).ToList();
            #endregion

            var list = await _blogRepository.GetListBlogFilterAsync(paging, filter, joinPosts);
            if (list == null)
            {
                return null;
            }
            var listBlogModels = _mapper.Map<List<BlogModel>>(list);
            return new Pagination<BlogModel>(listBlogModels, list.TotalCount, list.CurrentPage, list.PageSize);
        }

        public async Task<BlogModel> GetBlogByIdAsync(int Id)
        {
            var result = await _blogRepository.GetBlogByIdAsync(Id);
            if(result == null)
            {
                return null;
            }
            return _mapper.Map<BlogModel>(result);
        }

        public async Task<Pagination<BlogModel>> GetListBlogAsync(PaginationParameter paginationParameter)
        {
            var listBlog = await _blogRepository.GetListBlogAsync(paginationParameter);
            if(listBlog == null)
            {
                return null;
            }
            var listBlogModels = _mapper.Map<List<BlogModel>>(listBlog);
            return new Pagination<BlogModel>(listBlogModels, listBlog.TotalCount, listBlog.CurrentPage, listBlog.PageSize);
        }

        public async Task<bool> RemoveBlogAsync(int id)
        {
            var blog = await _blogRepository.GetBlogByIdAsync(id);
            if(blog == null)
            {
                return false;
            }
            else
            {
                await _blogRepository.DeleteBlogAsync(blog);
                return true;
            }
        }

        public async Task<BlogModel> UpdateBlogAsync(BlogModel blog)
        {
            if(blog == null)
            {
                return null;
            }
            var blogExist = await _blogRepository.GetBlogByIdAsync(blog.Id);
            if(blogExist == null)
            {
                return null;
            }
            else
            {
                blogExist.Date = blog.Date;
                blogExist.Title = blog.Title;
                blogExist.Content = blog.Content;
                blogExist.UserId = blog.UserId;
                blogExist.Banner = blog.Banner;
                blogExist.UnsignTitle = StringUtils.ConvertToUnSign(blog.Title);

                var updateBlog = _mapper.Map<Post>(blogExist);
                var result = await _blogRepository.UpdateBlogAsync(updateBlog);

                return _mapper.Map<BlogModel>(result);
            }
        }

        public async Task<List<BlogModel>> GetTopBlog()
        {
            var result = await _blogRepository.GetTopBlog();
            return _mapper.Map<List<BlogModel>>(result);
        }
    }
}
