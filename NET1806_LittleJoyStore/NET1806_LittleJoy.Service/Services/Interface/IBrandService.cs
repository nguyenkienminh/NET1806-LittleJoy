using NET1806_LittleJoy.Repository.Commons;
using NET1806_LittleJoy.Repository.Entities;
using NET1806_LittleJoy.Service.BusinessModels;


namespace NET1806_LittleJoy.Service.Services.Interface
{
    public interface IBrandService
    {
        public Task<Pagination<BrandModel>> GetAllBrandPagingAsync(PaginationParameter paginationParameter);

        public Task<BrandModel?> GetBrandByIdAsync(int brandId);

        public Task<bool?> AddBrandAsync(BrandModel brandModel);

        public Task<bool> RemoveBrandByIdAsync(int removeBrandById);

        public Task<BrandModel> UpdateBrandAsync(BrandModel brandModel);
    }
}
