using Microsoft.EntityFrameworkCore.Storage;
using NET1806_LittleJoy.API.ViewModels.RequestModels;
using NET1806_LittleJoy.Repository.Commons;
using NET1806_LittleJoy.Repository.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NET1806_LittleJoy.Repository.Repositories.Interface
{
    public interface IOrderRepository
    {
        public Task<Order> AddNewOrder(Order order);

        public Task<bool> AddNewOrderDetails(OrderDetail orderDetails);

        public Task<IDbContextTransaction> BeginTransactionAsync();

        public Task<Order> GetOrderById(int id);

        public Task<bool> UpdateOrder(Order order);

        public Task<Pagination<Order>> GetOrderByUserId(PaginationParameter parameter ,int userId);

        public Task<List<OrderDetail>> GetOrderDetailsByOrderId(int orderId);
    }
}
