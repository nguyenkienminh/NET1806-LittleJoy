using NET1806_LittleJoy.Repository.Commons;
using NET1806_LittleJoy.Service.BusinessModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NET1806_LittleJoy.Service.Services.Interface
{
    public interface IOriginService
    {
        public Task<Pagination<OriginModel>> GetAllOriginPagingAsync(PaginationParameter paginationParameter);

        public Task<OriginModel?> GetOriginByIdAsync(int originId);

        public Task<bool?> AddOriginAsync(OriginModel originModel);

        public Task<bool> RemoveOriginByIdAsync(int originId);

        public Task<OriginModel> UpdateOriginAsync(OriginModel originModel);
    }
}
