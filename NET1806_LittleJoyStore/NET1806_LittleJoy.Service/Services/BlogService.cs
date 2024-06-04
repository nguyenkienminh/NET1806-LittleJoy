using AutoMapper;
using NET1806_LittleJoy.Repository.Commons;
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

        public async Task<Pagination<BlogModel>> getListBlogAsync(PaginationParameter paginationParameter)
        {
            var listBlog = await _blogRepository.GetListBlogAsync(paginationParameter);
            if(listBlog == null)
            {
                return null;
            }
            var listBlogModels = _mapper.Map<List<BlogModel>>(listBlog);
            return new Pagination<BlogModel>(listBlogModels, listBlog.TotalCount, listBlog.CurrentPage, listBlog.PageSize);
        }
    }
}
