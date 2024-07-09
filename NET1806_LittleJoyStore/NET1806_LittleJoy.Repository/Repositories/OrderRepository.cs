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
                        query = query.Where(o => o.Status.Equals("Đặt Hàng Thành Công"));
                        break;

                    case 2:
                        query = query.Where(o => o.Status.Equals("Đã Hủy"));
                        break;
                }
            }

            if (filterModel.PaymentStatus.HasValue)
            {
                switch (filterModel.PaymentStatus)
                {
                    case 1:
                        query = query.Where(o => o.Payment.Status.Equals("Đang Chờ"));
                        break;

                    case 2:
                        query = query.Where(o => o.Payment.Status.Equals("Thành Công"));
                        break;

                    case 3:
                        query = query.Where(o => o.Payment.Status.Equals("Thất Bại"));
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
                query = query.Where(o => o.Payment.Code.ToString().Contains(filterModel.OrderCode.ToString()));
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
                        {
                            if (filterModel.SortPrice.HasValue)
                            {
                                switch (filterModel.SortPrice)
                                {
                                    case 1:
                                        query = query.OrderBy(o => o.Date).ThenBy(x => x.TotalPrice);
                                        break;

                                    case 2:
                                        query = query.OrderBy(o => o.Date).ThenByDescending(x => x.TotalPrice);
                                        break;
                                }
                            }
                            else
                            {
                                query = query.OrderBy(o => o.Date);
                            }
                            break;
                        }

                    case 2:
                        {
                            if (filterModel.SortPrice.HasValue)
                            {
                                switch (filterModel.SortPrice)
                                {
                                    case 1:
                                        query = query.OrderByDescending(o => o.Date).ThenBy(x => x.TotalPrice);
                                        break;

                                    case 2:
                                        query = query.OrderByDescending(o => o.Date).ThenByDescending(x => x.TotalPrice);
                                        break;
                                }
                            }
                            else
                            {
                                query = query.OrderByDescending(o => o.Date);
                            }
                            break;
                        }
                }
            }
            else if (filterModel.SortPrice.HasValue)
            {
                switch (filterModel.SortPrice)
                {
                    case 1:
                        {
                            query = query.OrderBy(x => x.TotalPrice);
                            break;
                        }
                    case 2:
                        {
                            query = query.OrderByDescending(x => x.TotalPrice);
                            break;
                        }
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


            if(filterModel.SortPrice.HasValue || filterModel.SortDate.HasValue)
            {
                var item = await query.Skip((paging.PageIndex - 1) * paging.PageSize)
                                     .Take(paging.PageSize)
                                     .AsNoTracking()
                                     .ToListAsync();

                return new Pagination<Order>(item, itemCount, paging.PageIndex, paging.PageSize);
            }
            else
            {
                var item = await query.OrderByDescending(x => x.Date).Skip((paging.PageIndex - 1) * paging.PageSize)
                                     .Take(paging.PageSize)
                                     .AsNoTracking()
                                     .ToListAsync();

                return new Pagination<Order>(item, itemCount, paging.PageIndex, paging.PageSize);
            }
            
        }

        public async Task<int> GetRevenueToday(DateTime currentDate)
        {
            var itemDate = _context.Orders.Include(p => p.Payment).Where(u => u.Date.HasValue && u.Date.Value.Date == currentDate.Date).AsQueryable();

            var itemStatus = itemDate.Where(u => u.Status.Trim() == "Đặt Hàng Thành Công" && u.DeliveryStatus.Trim() == "Giao Hàng Thành Công" && u.Payment.Status.Trim() == "Thành Công");

            var total = (int)await itemStatus.SumAsync(u => u.TotalPrice);

            return total;
        }

        public async Task<int> CountOrder(DateTime currentDate, bool status)
        {
            var itemDate = _context.Orders.Include(p => p.Payment).Where(u => u.Date.HasValue && u.Date.Value.Year == currentDate.Year && u.Date.Value.Month == currentDate.Month).AsQueryable();

            if (status)
            {
                var itemTrue = itemDate.Where(u => u.Status.Trim() != "Đã Hủy" && u.Payment.Status.Trim() != "Thất Bại");
                return (int)await itemTrue.CountAsync();
            }

            var itemFalse = itemDate.Where(u => u.Status.Trim() == "Đã Hủy" && u.DeliveryStatus == "Giao Hàng Thất Bại");
            return (int)await itemFalse.CountAsync();
        }

        public async Task<int> GetRevenueOverviewByMonth(DateTime currentDate, int month)
        {
            var itemDate = _context.Orders.Include(p => p.Payment).Where(u => u.Date.HasValue && u.Date.Value.Year == currentDate.Year && u.Date.Value.Month == month).AsQueryable();

            var itemStatus = itemDate.Where(u => u.Status.Trim() == "Đặt Hàng Thành Công" && u.DeliveryStatus.Trim() == "Giao Hàng Thành Công" && u.Payment.Status.Trim() == "Thành Công");

            var total = (int)await itemStatus.SumAsync(u => u.TotalPrice);

            return total;
        }

        public async Task<List<Order>> GetAllOrderWithCurrentDate(DateTime currentDate)
        {
            var itemDate = _context.Orders.Include(o => o.OrderDetails)
                                                .Where(u => u.Date.HasValue &&
                                                       u.Date.Value.Year == currentDate.Year &&
                                                       u.Date.Value.Month == currentDate.Month)
                                                .AsQueryable();

            var item = itemDate.Where(u => u.Status.Trim() == "Đặt Hàng Thành Công" && u.DeliveryStatus.Trim() == "Giao Hàng Thành Công" && u.Payment.Status.Trim() == "Thành Công");

            return await item.ToListAsync();
        }

    }
}
