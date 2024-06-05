using NET1806_LittleJoy.Repository.Commons;
using NET1806_LittleJoy.Service.BusinessModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NET1806_LittleJoy.Service.Services.Interface
{
    public interface ICategoryService
    {
        public Task<Pagination<CategoryModel>> GetAllCategoryPagingAsync(PaginationParameter paginationParameter);

        public Task<CategoryModel?> GetCategoryByIdAsync(int cateId);

        public Task<bool?> AddCategoryAsync(CategoryModel categoryModel);

        public Task<bool> RemoveCategoryByIdAsync(int cateId);

        public Task<CategoryModel> UpdateCategoryAsync(CategoryModel cateModel);

    }
}
