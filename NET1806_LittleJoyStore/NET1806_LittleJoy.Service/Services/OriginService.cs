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
    public  class OriginService : IOriginService
    {
        private readonly IOriginRepository _originRepo;
        private readonly IMapper _mapper;

        public OriginService(IOriginRepository originRepo, IMapper mapper)
        {
            _originRepo = originRepo;
            _mapper = mapper;
        }

        public async Task<Pagination<OriginModel>> GetAllOriginPagingAsync(PaginationParameter paginationParameter)
        {
            var listOrigins = await _originRepo.GetAllOriginPagingAsync(paginationParameter);

            if (!listOrigins.Any())
            {
                return null;
            }

            var listOriginModels = listOrigins.Select(o => new OriginModel
            {
                Id = o.Id,
                OriginName = o.OriginName,
            }).ToList();

            return new Pagination<OriginModel>(listOriginModels,
                listOrigins.TotalCount,
                listOrigins.CurrentPage,
                listOrigins.PageSize);
        }

        public async Task<OriginModel?> GetOriginByIdAsync(int originId)
        {
            var origin = await _originRepo.GetOriginByIdAsync(originId);

            if (origin == null)
            {
                return null;
            }

            return _mapper.Map<OriginModel>(origin);

            
        }

        public async Task<bool?> AddOriginAsync(OriginModel originModel)
        {
            try
            {
                var originInfo = _mapper.Map<Origin>(originModel);
                var item = await _originRepo.AddOriginAsync(originInfo);

                if (item == null)
                {
                    return false;
                }
                return true;

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Fail to add Origin {ex.Message}");
                return false;
            }
        }

        public async Task<bool> RemoveOriginByIdAsync(int originId)
        {
            var productsOrigin = await _originRepo.GetProductsByOriginIdAsync(originId);

            if (productsOrigin.Any())
            {
                return false;
            }

            var item = await _originRepo.GetOriginByIdAsync(originId);

            if (item == null)
            {
                return false;
            }

            return await _originRepo.RemoveOriginAsync(item);
        }

        public async Task<OriginModel> UpdateOriginAsync(OriginModel originModel)
        {
            var originModify = _mapper.Map<Origin>(originModel);

            var originPlace = await _originRepo.GetOriginByIdAsync(originModel.Id);

            if (originPlace == null)
            {
                return null;
            }

            var updateOrigin = await _originRepo.UpdateOriginAsync(originModify, originPlace);

            if (updateOrigin != null)
            {
                return _mapper.Map<OriginModel>(updateOrigin);
            }

            return null;
        }
    }
}
