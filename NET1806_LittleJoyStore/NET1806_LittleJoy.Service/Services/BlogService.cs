using AutoMapper;
using NET1806_LittleJoy.Repository.Commons;
using NET1806_LittleJoy.Repository.Entities;
using NET1806_LittleJoy.Repository.Repositories;
using NET1806_LittleJoy.Repository.Repositories.Interface;
using NET1806_LittleJoy.Service.BusinessModels;
using NET1806_LittleJoy.Service.Services.Interface;
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

        public BlogService(IBlogRepository blogRepository, IMapper mapper) 
        {
            _blogRepository = blogRepository;
            _mapper = mapper;
        }

        public async Task<BlogModel> CreateNewBlog(BlogModel model)
        {
            if(model == null)
            {
                return null;
            }
            var blogModel = _mapper.Map<Post>(model);
            var blog = await _blogRepository.CreateNewBlog(blogModel);
            if(blog != null)
            {
                return _mapper.Map<BlogModel>(blog);
            }
            else
            {
                return null;
            }
            
        }

        public async Task<BlogModel> GetBlogById(int Id)
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

        public async Task<bool> RemoveBlog(int id)
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

        public async Task<BlogModel> UpdateBlog(BlogModel blog)
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

                var updateBlog = _mapper.Map<Post>(blogExist);
                var result = await _blogRepository.UpdateBlogAsync(updateBlog);

                return _mapper.Map<BlogModel>(result);
            }
        }
    }
}
