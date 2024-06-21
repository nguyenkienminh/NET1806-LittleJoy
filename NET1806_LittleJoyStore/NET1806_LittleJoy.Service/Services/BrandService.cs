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
    public class BrandService : IBrandService
    {
        private readonly IBrandRepository _brandRepository;
        private readonly IMapper _mapper;

        public BrandService(IBrandRepository brandRepository, IMapper mapper)
        {
            _brandRepository = brandRepository;
            _mapper = mapper;
        }


        public async Task<Pagination<BrandModel>> GetAllBrandPagingAsync(PaginationParameter paginationParameter)
        {
            var listBrands = await _brandRepository.GetAllBrandPagingAsync(paginationParameter);

            if (!listBrands.Any())
            {
                return null;
            }

            var listBrandModels = listBrands.Select(b => new BrandModel
            {
                Id = b.Id,
                BrandName = b.BrandName,
                Logo = b.Logo,
                BrandDescription = b.BrandDescription,
            }).ToList();


            return new Pagination<BrandModel>(listBrandModels,
                listBrands.TotalCount,
                listBrands.CurrentPage,
                listBrands.PageSize);
        }


        public async Task<BrandModel?> GetBrandByIdAsync(int brandId)
        {
            var brandDetail = await _brandRepository.GetBrandByIdAsync(brandId);

            if (brandDetail == null)
            {
                return null;
            }

            var brandDetailModel = _mapper.Map<BrandModel>(brandDetail);

            return brandDetailModel;
        }


        public async Task<bool?> AddBrandAsync(BrandModel brandModel)
        {
            try
            {
                if (brandModel.BrandName != null)
                {

                    if (brandModel.BrandName.Equals(""))
                    {
                        throw new Exception("Không được tạo BrandName trống");
                    }

                    var listBrand = await _brandRepository.GetAllBrandAsync();

                    foreach (var brand in listBrand)
                    {
                        if (brand.BrandName.Equals(brandModel.BrandName))
                        {
                            throw new Exception("Không được tạo BrandName trùng lặp");
                        }
                    }

                    var brandInfo = _mapper.Map<Brand>(brandModel);
                    var item = await _brandRepository.AddBrandAsync(brandInfo);

                    if (item == null)
                    {
                        return false;
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                
                throw;
            }
        }


        public async Task<bool> RemoveBrandByIdAsync(int removeBrandById)
        {
            var productsBrand = await _brandRepository.GetProductsByBrandIdAsync(removeBrandById);

            if (productsBrand.Any())
            {
                return false;
            }

            var item = await _brandRepository.GetBrandByIdAsync(removeBrandById);

            if (item == null)
            {
                return false;
            }

            return await _brandRepository.RemoveBrandAsync(item);
        }


        public async Task<BrandModel> UpdateBrandAsync(BrandModel brandModel)
        {

            if (brandModel.BrandName != null)
            {

                if (brandModel.BrandName.Equals(""))
                {
                    throw new Exception("Không được tạo BrandName trống");
                }

                var listBrand = await _brandRepository.GetAllBrandAsync();

                foreach (var brand in listBrand)
                {
                    if (brand.BrandName.Equals(brandModel.BrandName) && brand.Id != brandModel.Id)
                    {
                        throw new Exception("Không được thay đổi BrandName trùng lặp");
                    }
                }

                var brandModify = _mapper.Map<Brand>(brandModel);

                var brandPlace = await _brandRepository.GetBrandByIdAsync(brandModel.Id);

                if (brandPlace == null)
                {
                    return null;
                }
                else
                {
                    brandPlace.Id = brandModify.Id;
                    brandPlace.BrandName = brandModify.BrandName;
                    brandPlace.BrandDescription = brandModify.BrandDescription;
                    brandPlace.Logo = brandModify.Logo;
                    var updateBrand = await _brandRepository.UpdateBrandAsync(brandPlace);

                    if (updateBrand != null)
                    {
                        return _mapper.Map<BrandModel>(updateBrand);
                    }
                }
            }
            return null;
        }
    }
}
