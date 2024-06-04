using NET1806_LittleJoy.Repository.Entities;
using NET1806_LittleJoy.Service.BusinessModels;


namespace NET1806_LittleJoy.Service.Services.Interface
{
    public interface IBrandService
    {
        public Task<Brand> AddBrandAsync(BrandModel brandmodel);
    }
}
