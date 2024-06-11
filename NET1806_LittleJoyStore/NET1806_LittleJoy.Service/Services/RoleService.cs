using AutoMapper;
using NET1806_LittleJoy.Repository.Commons;
using NET1806_LittleJoy.Repository.Entities;
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
    public class RoleService: IRoleService
    {
        private readonly IRoleRepository _repo;
        private readonly IMapper _mapper;

        public RoleService(IRoleRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<List<RoleModel>> GetAllRoleAsync()
        {
            var listRole = await _repo.GetAllRoleAsync();

            if (!listRole.Any())
            {
                return null;
            }

            var models = listRole.Select(r => new RoleModel
            {
                Id = r.Id,
                RoleName = r.RoleName,
            }).ToList();

            return models;
        }

        public async Task<RoleModel> GetRoleByIdAsync(int id)
        {
            var roleDetail = await _repo.GetRoleByIdAsync(id);

            if (roleDetail == null)
            {
                return null;
            }

            var model = _mapper.Map<RoleModel>(roleDetail);

            return model;
        }


        public async Task<bool> AddRoleAsync(RoleModel model)
        {
            try
            {
                var roleInfo = _mapper.Map<Role>(model);
                var item = await _repo.AddRoleAsync(roleInfo);

                if (item == null)
                {
                    return false;
                }
                return true;

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Fail to add Role {ex.Message}");
                return false;
            }
        }

        public async Task<bool> RemoveRoleByIdAsync(int Id)
        {
            var users = await _repo.GetUserByRoleIdAsync(Id);

            if (users.Any())
            {
                return false;
            }

            var item = await _repo.GetRoleByIdAsync(Id);

            if (item == null)
            {
                return false;
            }

            return await _repo.RemoveRoleByIdAsync(item);
        }

        public async Task<RoleModel> UpdateRoleAsync(RoleModel model)
        {
            var roleModify = _mapper.Map<Role>(model);

            var rolePlace = await _repo.GetRoleByIdAsync(model.Id);

            if (rolePlace == null)
            {
                return null;
            }

            var updateRole = await _repo.UpdateRoleAsync(roleModify, rolePlace);

            if (updateRole != null)
            {
                return _mapper.Map<RoleModel>(updateRole);
            }

            return null;
        }
    }
}
