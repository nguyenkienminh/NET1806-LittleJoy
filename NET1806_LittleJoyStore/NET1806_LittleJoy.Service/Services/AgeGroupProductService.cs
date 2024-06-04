using AutoMapper;
using NET1806_LittleJoy.Repository.Commons;
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
    public class AgeGroupProductService : IAgeGroupProductService
    {
        private readonly IAgeGroupProductRepository _ageGroup;
        private readonly IMapper _mapper;

        public AgeGroupProductService(IAgeGroupProductRepository ageGroupProductRepository, IMapper mapper)
        {
            _ageGroup = ageGroupProductRepository;
            _mapper = mapper;
        }

        public async Task<Pagination<AgeGroupProductModel>> GetAllAgeGroupPagingAsync(PaginationParameter paginationParameter)
        {
            var listAgeGroup = await _ageGroup.GetAllAgeGroupPagingAsync(paginationParameter);
            if (!listAgeGroup.Any())
            {
                return null;
            }

            var listAgeGroupModels = listAgeGroup.Select(ag => new AgeGroupProductModel
            {
                Id = ag.Id,
                AgeRange= ag.AgeRange
            }).ToList();


            return new Pagination<AgeGroupProductModel>(listAgeGroupModels,
                listAgeGroup.TotalCount,
                listAgeGroup.CurrentPage,
                listAgeGroup.PageSize);
        }

        public async Task<AgeGroupProductModel?> GetAgeGroupByIdAsync(int ageID)
        {
            var item = await _ageGroup.GetAgeGroupByIdAsync(ageID);

            if (item == null)
            {
                return null;
            }

             return _mapper.Map<AgeGroupProductModel>(item);

            
        }
    }
}
