using NET1806_LittleJoy.Repository.Commons;
using NET1806_LittleJoy.Repository.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NET1806_LittleJoy.Repository.Repositories.Interface
{
    public  interface IOriginRepository
    {
        public Task<Pagination<Origin>> GetAllOriginPagingAsync(PaginationParameter paginationParameter);

        public Task<Origin?> GetOriginByIdAsync(int originId);

        public Task<Origin> AddOriginAsync(Origin origin);

        public Task<bool> RemoveOriginAsync(Origin origin);

        public Task<ICollection<Product>> GetProductsByOriginIdAsync(int originId);

        public Task<Origin> UpdateOriginAsync(Origin originModify);

        public Task<ICollection<Origin>> GetAllOriginAsync();
    }
}
