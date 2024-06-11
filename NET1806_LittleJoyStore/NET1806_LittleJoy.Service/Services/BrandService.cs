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
                var brandInfo = _mapper.Map<Brand>(brandModel);
                var item =  await _brandRepository.AddBrandAsync(brandInfo);
                
                if(item == null)
                {
                    return false;
                }
                return true;

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Fail to add Brand {ex.Message}");
                return false;
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
            var brandModify = _mapper.Map<Brand>(brandModel);

            var brandPlace = await _brandRepository.GetBrandByIdAsync(brandModel.Id);

            if (brandPlace == null)
            {
                return null;
            }

            var updateBrand = await _brandRepository.UpdateBrandAsync(brandModify, brandPlace);

            if (updateBrand != null)
            {
                return _mapper.Map<BrandModel>(updateBrand);
            }
            return null;
        }
    }
}
