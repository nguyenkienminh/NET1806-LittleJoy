using NET1806_LittleJoy.Repository.Commons;
using NET1806_LittleJoy.Service.BusinessModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NET1806_LittleJoy.Service.Services.Interface
{
    public interface IProductService
    {
        public Task<Pagination<ProductModel>> GetAllProductPagingAsync(PaginationParameter paginationParameter);

        public Task<ProductModel?> GetProductByIdAsync(int productId);

        public Task<bool> AddNewProductAsync(ProductModel productModel);

        public Task<bool> DeleteProductByIdAsync(int productId);

        public Task<ProductModel> UpdateProductAsync(ProductModel productModel);

        public Task<Pagination<ProductModel>> FilterProductPagingAsync(PaginationParameter paging, ProductFilterModel model);

        public Task<Pagination<ProductModel>> FilterStatusProductPagingAsync(PaginationParameter paging, ProductFilterStatusModel filterStatus);
    }
}
