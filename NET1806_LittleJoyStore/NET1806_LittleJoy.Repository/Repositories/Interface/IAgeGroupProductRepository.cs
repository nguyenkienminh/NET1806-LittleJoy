using NET1806_LittleJoy.Repository.Commons;
using NET1806_LittleJoy.Repository.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NET1806_LittleJoy.Repository.Repositories.Interface
{
    public interface IAgeGroupProductRepository
    {
        public Task<Pagination<AgeGroupProduct>> GetAllAgeGroupPagingAsync(PaginationParameter paginationParameter);

        public Task<AgeGroupProduct?> GetAgeGroupByIdAsync(int ageId);
    }
}
