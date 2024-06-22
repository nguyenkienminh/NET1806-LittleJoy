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

            var listAddressModel = listAddress.Select(a => new AddressModel
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

                if (countAddress == 0)
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
            try
            {
                var item = await _repo.GetAddressByIdAsync(id);

                if (item == null )
                {
                    throw new Exception("Address không tồn tại");
                }
                else
                {
                    if(item.IsMainAddress == true)
                    {
                        throw new Exception("Phải luôn có 1 address chính");
                    }
                }

                return await _repo.DeleteAddressAsync(item);
            }
            catch (Exception ex)
            { 
                throw ex;
            }

        }

        public async Task<AddressModel> UpdateAddressAsync(AddressModel model)
        {

            try
            {
                Address? modifyAddress = null;

                var addressDetailUpdate = _mapper.Map<Address>(model); // chuyển thành Address

                var addressPlace = await _repo.GetAddressByIdAsync(addressDetailUpdate.Id); //Lấy thông tin cũ của vị trí muốn update

                if (addressPlace == null) // Nếu vị trí là null
                {
                    throw new Exception("Address không tồn tại");
                }

                addressDetailUpdate.UserId = addressPlace.UserId; // gắn giá trị UserId cho cái update

                //check và tiến hành update
                #region Update Address
                if (addressDetailUpdate.IsMainAddress == false) // Nếu cái Update là false
                {

                    if (addressPlace.IsMainAddress == true)
                    {
                        // Nếu thông tin ban đầu là true
                        throw new Exception("Phải luôn có 1 address chính");
                    }
                    else
                    {
                        // Nếu thông tin ban đầu là false
                        modifyAddress = await _repo.UpdateAddressAsync(addressDetailUpdate, addressPlace);
                    }
                }
                else // Nếu cái update là true
                {
                    var listAddress = await _repo.GetAddressByUserIdAsync(addressDetailUpdate.UserId);  // lấy danh sách đỉa chỉ của UserId

                    foreach (var item in listAddress)
                    {
                        if (item.Id != addressDetailUpdate.Id)  // Nếu Cái Id của danh sách mà khác cái Id update --> Chuyển thành false và Update lại
                        {
                            var change = await _repo.GetAddressByIdAsync(item.Id);

                            if (change != null)
                            {
                                change.IsMainAddress = false;

                                await _repo.UpdateAddressAsync(item, change);
                            }

                        }
                    }

                    modifyAddress = await _repo.UpdateAddressAsync(addressDetailUpdate, addressPlace); // Sau khi các thằng khác đã false thì update cái mói vào Ismain là true
                }
                #endregion

                if (modifyAddress != null)
                {
                    return _mapper.Map<AddressModel>(modifyAddress);
                }

                return null;
            }catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<Pagination<AddressModel>> GetAddressListPagingByUserIdAsync(PaginationParameter paging, int id)
        {
            var listAddress = await _repo.GetAddressListPagingByUserIdAsync(paging,id);

            if (!listAddress.Any())
            {
                return null;
            }

            var listAddressModel = listAddress.Select(a => new AddressModel
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

        public async Task<AddressModel> GetMainAddressByUserIdAsync(int userId)
        {
            var addressMain = await _repo.GetMainAddressByUserIdAsync(userId);

            if (addressMain == null)
            {
                return null;
            }

            var addressMainModel = _mapper.Map<AddressModel>(addressMain);

            return addressMainModel;
        }
    }
}
