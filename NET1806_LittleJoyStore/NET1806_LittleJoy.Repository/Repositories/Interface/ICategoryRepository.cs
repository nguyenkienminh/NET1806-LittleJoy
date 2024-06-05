using NET1806_LittleJoy.Repository.Commons;
using NET1806_LittleJoy.Repository.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NET1806_LittleJoy.Repository.Repositories.Interface
{
    public  interface ICategoryRepository
    {
        public Task<Pagination<Category>> GetAllCategoryPagingAsync(PaginationParameter paginationParameter);

        public Task<Category?> GetCategoryByIdAsync(int cateId);

        public Task<Category> AddCategoryAsync(Category category);

        public Task<bool> RemoveCategoryAsync(Category category);

        public Task<ICollection<Product>> GetProductsByCategoryIdAsync(int cateId);

        public Task<Category> UpdateCategoryAsync(Category cateModify, Category catePlace);
    }
}
