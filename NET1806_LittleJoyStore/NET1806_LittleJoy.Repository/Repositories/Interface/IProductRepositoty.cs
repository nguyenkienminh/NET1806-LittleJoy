using NET1806_LittleJoy.Repository.Commons;
using NET1806_LittleJoy.Repository.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NET1806_LittleJoy.Repository.Repositories.Interface
{
    public interface IProductRepositoty
    {
        public Task<Pagination<Product>> GetAllProductPagingAsync(PaginationParameter paginationParameter);

    }
}
