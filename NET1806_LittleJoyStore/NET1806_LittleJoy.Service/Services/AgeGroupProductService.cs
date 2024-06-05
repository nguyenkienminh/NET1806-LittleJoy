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
    public class AgeGroupProductService : IAgeGroupProductService
    {
        private readonly IAgeGroupProductRepository _ageGroupRepo;
        private readonly IMapper _mapper;

        public AgeGroupProductService(IAgeGroupProductRepository ageGroupProductRepository, IMapper mapper)
        {
            _ageGroupRepo = ageGroupProductRepository;
            _mapper = mapper;
        }

        public async Task<Pagination<AgeGroupProductModel>> GetAllAgeGroupPagingAsync(PaginationParameter paginationParameter)
        {
            var listAgeGroup = await _ageGroupRepo.GetAllAgeGroupPagingAsync(paginationParameter);
            if (!listAgeGroup.Any())
            {
                return null;
            }

            var listAgeGroupModels = listAgeGroup.Select(ag => new AgeGroupProductModel
            {
                Id = ag.Id,
                AgeRange = ag.AgeRange
            }).ToList();


            return new Pagination<AgeGroupProductModel>(listAgeGroupModels,
                listAgeGroup.TotalCount,
                listAgeGroup.CurrentPage,
                listAgeGroup.PageSize);
        }

        public async Task<AgeGroupProductModel?> GetAgeGroupByIdAsync(int ageId)
        {
            var item = await _ageGroupRepo.GetAgeGroupByIdAsync(ageId);

            if (item == null)
            {
                return null;
            }

            return _mapper.Map<AgeGroupProductModel>(item);


        }

        public async Task<bool?> AddAgeGroupAsync(AgeGroupProductModel ageGroup)
        {
            try
            {
                var brandInfo = _mapper.Map<AgeGroupProduct>(ageGroup);
                var item = await _ageGroupRepo.AddAgeGroupAsync(brandInfo);

                if (item == null)
                {
                    return false;
                }
                return true;

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Fail to add Age {ex.Message}");
                return false;
            }
        }

        public async Task<bool> RemoveAgeGroupByIdAsync(int ageId)
        {
            var productsAge = await _ageGroupRepo.GetProductsByAgeIdAsync(ageId);

            if (productsAge.Any())
            {
                return false;
            }

            var item = await _ageGroupRepo.GetAgeGroupByIdAsync(ageId);

            if (item == null)
            {
                return false;
            }

            return await _ageGroupRepo.RemoveAgeGroupAsync(item);
        }

        public async Task<AgeGroupProductModel> UpdateAgeGroupAsync(AgeGroupProductModel ageGroup)
        {
            var ageModify = _mapper.Map<AgeGroupProduct>(ageGroup);

            var agePlace = await _ageGroupRepo.GetAgeGroupByIdAsync(ageGroup.Id);

            if (agePlace == null)
            {
                return null;
            }

            else
            {
                var updateAge = await _ageGroupRepo.UpdateAgeGroupAsync(ageModify, agePlace);

                if (updateAge != null)
                {
                    return _mapper.Map<AgeGroupProductModel>(updateAge);
                }
            }

            return null;
        }
    }
}
