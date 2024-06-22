using Microsoft.AspNetCore.Http;
using NET1806_LittleJoy.API.ViewModels.RequestModels;
using NET1806_LittleJoy.Repository.Commons;
using NET1806_LittleJoy.Service.BusinessModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NET1806_LittleJoy.Service.Services.Interface
{
    public interface IOrderService
    {
        public Task<OrderResponseModel> CreateOrder(OrderRequestModel model, HttpContext context);

        public Task<Pagination<OrderWithDetailsModel>> GetOrderByUserId(PaginationParameter parameter, int userId);
    }
}
