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
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace NET1806_LittleJoy.Service.Services
{
    public class AddressService : IAddressService
    {
        private readonly IAddressRepository _repo;
        private readonly IMapper _mapper;

        public AddressService(IAddressRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<Pagination<AddressModel>> GetAllPagingAddressAsync(PaginationParameter paging)
        {
            var listAddress = await _repo.GetAllPagingAddressAsync(paging);

            if (!listAddress.Any())
            {
                return null;
            }

            var listAddressModel= listAddress.Select(a => new AddressModel
            {
                Id = a.Id,
                Address1 = a.Address1,
                IsMainAddress = a.IsMainAddress,
                UserId = a.UserId,
                
            }).ToList();


            return new Pagination<AddressModel>(listAddressModel,
                listAddress.TotalCount,
                listAddress.CurrentPage,
                listAddress.PageSize);
        }

        public async Task<AddressModel?> GetAddressByIdAsync(int id)
        {
            var addressDetail = await _repo.GetAddressByIdAsync(id);

            if (addressDetail == null)
            {
                return null;
            }

            var addressDetailModel = _mapper.Map<AddressModel>(addressDetail);

            return addressDetailModel;
        }

        public async Task<bool?> AddAddressAsync(AddressModel model)
        {
            try
            {
                var addressInfo = _mapper.Map<Address>(model);

                var countAddress = await _repo.CountAddressByUserIdAsync(model.UserId);

                if(countAddress == 0)
                {
                    addressInfo.IsMainAddress = true;
                }
                else
                {
                    addressInfo.IsMainAddress = false;  
                }

                var item = await _repo.AddAddressAsync(addressInfo);

                if (item == null)
                {
                    return false;
                }
                return true;

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Fail to add Address {ex.Message}");
                return false;
            }
        }

        public async Task<bool> DeleteAddressByIdAsync(int id)
        {

            var item = await _repo.GetAddressByIdAsync(id);

            if (item == null || item.IsMainAddress == true)
            {
                return false;
            }

            return await _repo.DeleteAddressAsync(item);

        }
    }
}
