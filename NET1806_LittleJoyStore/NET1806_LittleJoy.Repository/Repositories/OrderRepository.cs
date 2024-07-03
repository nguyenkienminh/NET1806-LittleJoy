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
            var items = await query.OrderByDescending(x => x.Id)
                                    .Skip((paging.PageIndex - 1) * paging.PageSize)
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

        public async Task<Pagination<Order>> OrderFilterAsync(PaginationParameter paging, OrderFilterModel filterModel)
        {
            var query = _context.Orders.Include(o => o.OrderDetails)
                                       .Include(o => o.Payment)
                                       .Include(o => o.User)
                                       .AsQueryable();

            if (filterModel.Status.HasValue)
            {
                switch (filterModel.Status)
                {
                    case 1:
                        query = query.Where(o => o.Status.Equals("Đang chờ"));
                        break;

                    case 2:
                        query = query.Where(o => o.Status.Equals("Đặt Hàng Thành Công"));
                        break;

                    case 3:
                        query = query.Where(o => o.Status.Equals("Đã hủy"));
                        break;
                }
            }

            if (filterModel.PaymentStatus.HasValue)
            {
                switch (filterModel.PaymentStatus)
                {
                    case 1:
                        query = query.Where(o => o.Payment.Status.Equals("Đang chờ"));
                        break;

                    case 2:
                        query = query.Where(o => o.Payment.Status.Equals("Thành công"));
                        break;

                    case 3:
                        query = query.Where(o => o.Payment.Status.Equals("Thất bại"));
                        break;
                }
            }

            if (filterModel.DeliveryStatus.HasValue)
            {
                switch (filterModel.DeliveryStatus)
                {
                    case 1:
                        query = query.Where(o => o.DeliveryStatus.Equals("Đang Chuẩn Bị"));
                        break;

                    case 2:
                        query = query.Where(o => o.DeliveryStatus.Equals("Đang Giao Hàng"));
                        break;

                    case 3:
                        query = query.Where(o => o.DeliveryStatus.Equals("Giao hàng thất bại"));
                        break;

                    case 4:
                        query = query.Where(o => o.DeliveryStatus.Equals("Giao Hàng Thành Công"));
                        break;

                    case 5:
                        query = query.Where(o => o.DeliveryStatus.Equals(""));
                        break;
                }
            }

            if (filterModel.OrderCode.HasValue)
            {
                query = query.Where(o => o.Payment.Code == filterModel.OrderCode);
            }

            if (!string.IsNullOrEmpty(filterModel.UserName))
            {
                query = query.Where(o => o.User.UserName.Contains(filterModel.UserName));
            }

            if (filterModel.SortDate.HasValue)
            {
                switch (filterModel.SortDate)
                {
                    case 1:
                        query = query.OrderBy(o => o.Date);
                        break;

                    case 2:
                        query = query.OrderByDescending(o => o.Date);
                        break;
                }
            }

            if (filterModel.SortPrice.HasValue)
            {
                switch (filterModel.SortPrice)
                {
                    case 1:
                        query = query.OrderBy(o => o.TotalPrice);
                        break;

                    case 2:
                        query = query.OrderByDescending(o => o.TotalPrice);
                        break;
                }
            }

                if (filterModel.PaymentMethod.HasValue)
                {
                    switch (filterModel.PaymentMethod)
                    {
                        case 1:
                            query = query.Where(o => o.Payment.Method.Equals("COD"));
                            break;

                        case 2:
                            query = query.Where(o => o.Payment.Method.Equals("VNPAY"));
                            break;
                    }
                }
            

            var itemCount = await query.CountAsync();

            var item = await query.Skip((paging.PageIndex - 1) * paging.PageSize)
                                     .Take(paging.PageSize)
                                     .OrderByDescending(o => o.Id)
                                     .AsNoTracking()
                                     .ToListAsync();

            var result = new Pagination<Order>(item, itemCount, paging.PageIndex, paging.PageSize);

            return result;
        }

    }
}
