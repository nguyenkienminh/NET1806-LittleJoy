using AutoMapper;
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

        public async Task<Brand> AddBrandAsync(BrandModel brandModel)
        {
            if (brandModel == null)
            {
                return null;
            }
            var brand = _mapper.Map<Brand>(brandModel);

            var result = await _brandRepository.AddBrandAsync(brand);

            if (result != null)
            {
                return brand;
            }
            else
            {
                return null;
            }
        }
    }
}
