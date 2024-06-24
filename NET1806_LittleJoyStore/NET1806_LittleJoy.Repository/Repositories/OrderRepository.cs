using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using NET1806_LittleJoy.API.ViewModels.RequestModels;
using NET1806_LittleJoy.Repository.Commons;
using NET1806_LittleJoy.Repository.Entities;
using NET1806_LittleJoy.Repository.Repositories.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
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

        public async Task<bool> UpdateOrder(Order order)
        {
            _context.Orders.Update(order);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<Pagination<Order>> GetOrderByUserId(PaginationParameter paging, int userId)
        {
            var query = _context.Orders.Where(x => x.UserId == userId).AsQueryable();

            var itemCount = await query.CountAsync();
            var items = await query.Skip((paging.PageIndex - 1) * paging.PageSize)
                                    .Take(paging.PageSize)
                                    .AsNoTracking().ToListAsync();

            var result = new Pagination<Order>(items, itemCount, paging.PageIndex, paging.PageSize);
            return result;
        }

        public async Task<List<OrderDetail>> GetOrderDetailsByOrderId(int orderId)
        {
            return await _context.OrderDetails.Where(x => x.OrderId == orderId).ToListAsync();
        }

        public async Task<List<Order>> GetOrderListByUserIdAsync(int userId)
        {
            var query = _context.Orders.Where(x => x.UserId == userId).AsQueryable();
            return await query.ToListAsync();
        }
    }
}
