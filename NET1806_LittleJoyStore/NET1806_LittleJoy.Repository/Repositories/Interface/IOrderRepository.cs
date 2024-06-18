using NET1806_LittleJoy.API.ViewModels.RequestModels;
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
    }
}
