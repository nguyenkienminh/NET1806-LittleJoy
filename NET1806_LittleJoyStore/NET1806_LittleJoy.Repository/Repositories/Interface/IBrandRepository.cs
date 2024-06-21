using NET1806_LittleJoy.Repository.Commons;
using NET1806_LittleJoy.Repository.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NET1806_LittleJoy.Repository.Repositories.Interface
{
    public interface IBrandRepository
    {
        
        public Task<Pagination<Brand>> GetAllBrandPagingAsync(PaginationParameter paginationParameter);

        public Task<Brand?> GetBrandByIdAsync(int brandId);

        public Task<Brand> AddBrandAsync(Brand brand);

        public Task<bool> RemoveBrandAsync(Brand brand);

        public Task<ICollection<Product>> GetProductsByBrandIdAsync(int brandId);

        public Task<Brand> UpdateBrandAsync(Brand brandModify);

        public Task<ICollection<Brand>> GetAllBrandAsync();
    }
}
