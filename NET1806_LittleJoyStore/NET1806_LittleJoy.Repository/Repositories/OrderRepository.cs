using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using NET1806_LittleJoy.API.ViewModels.RequestModels;
using NET1806_LittleJoy.Repository.Entities;
using NET1806_LittleJoy.Repository.Repositories.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NET1806_LittleJoy.Repository.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly LittleJoyContext _context;

        public OrderRepository(LittleJoyContext context) 
        {
            _context = context;
        }

        public async Task<IDbContextTransaction> BeginTransactionAsync()
        {
            return await _context.Database.BeginTransactionAsync();
        }

        public async Task<Order> AddNewOrder(Order order)
        {
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();
            return order;
        }

        public async Task<bool> AddNewOrderDetails(OrderDetail orderDetails)
        {
            _context.OrderDetails.Add(orderDetails);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<Order> GetOrderById(int id)
        {
            return await _context.Orders.Where(x => x.Id == id).FirstOrDefaultAsync();
        }
    }
}
