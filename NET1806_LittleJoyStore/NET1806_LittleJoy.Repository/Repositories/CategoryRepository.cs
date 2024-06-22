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
    public class CategoryRepository : ICategoryRepository
    {
        private readonly LittleJoyContext _context;

        public CategoryRepository(LittleJoyContext context)
        {
            _context = context;
        }

        public async Task<Pagination<Category>> GetAllCategoryPagingAsync(PaginationParameter paginationParameter)
        {
            var itemCount = await _context.Categories.CountAsync();

            var item = await _context.Categories.Skip((paginationParameter.PageIndex - 1) * paginationParameter.PageSize)
                                            .Take(paginationParameter.PageSize)
                                            .AsNoTracking()
                                            .ToListAsync();

            var result = new Pagination<Category>(item, itemCount, paginationParameter.PageIndex, paginationParameter.PageSize);

            return result;
        }

        public async Task<Category?> GetCategoryByIdAsync(int cateId)
        {
            return await _context.Categories.SingleOrDefaultAsync(c => c.Id == cateId);
        }

        public async Task<Category> AddCategoryAsync(Category category)
        {
            _context.Categories.Add(category);  

            await _context.SaveChangesAsync();

            return category;
        }

        public async Task<bool> RemoveCategoryAsync(Category category)
        {
            _context.Categories.Remove(category);

            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<ICollection<Product>> GetProductsByCategoryIdAsync(int cateId)
        {
            return await _context.Products.Where(p => p.CateId == cateId).ToListAsync();    
        }

        public async Task<Category> UpdateCategoryAsync(Category cateModify)
        {
            _context.Categories.Update(cateModify);
            await _context.SaveChangesAsync();

            return cateModify;
        }

        public async Task<ICollection<Category>> GetAllCateAsync()
        {
            return await _context.Categories.ToListAsync();
        }
    }
}
