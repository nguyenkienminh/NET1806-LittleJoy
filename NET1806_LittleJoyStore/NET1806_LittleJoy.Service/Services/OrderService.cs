using AutoMapper;
using NET1806_LittleJoy.API.ViewModels.RequestModels;
using NET1806_LittleJoy.Repository.Entities;
using NET1806_LittleJoy.Repository.Repositories;
using NET1806_LittleJoy.Repository.Repositories.Interface;
using NET1806_LittleJoy.Service.BusinessModels;
using NET1806_LittleJoy.Service.Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NET1806_LittleJoy.Service.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IProductRepositoty _productRepositoty;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public OrderService(IOrderRepository orderRepository, IProductRepositoty productRepositoty, IUserRepository userRepository, IMapper mapper)
        {
            _orderRepository = orderRepository;
            _productRepositoty = productRepositoty;
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<bool> CreateOrder(OrderRequestModel model)
        {
            using (var transaction = await _orderRepository.BeginTransactionAsync())
            {
                try
                {
                    var user = await _userRepository.GetUserByIdAsync(model.UserId);
                    if (user == null)
                    {
                        throw new Exception("Không tìm thấy user");
                    }
                    var orderModel = new OrderModel()
                    {
                        UserId = user.Id,
                        TotalPrice = model.TotalPrice,
                        Address = model.Address,
                        Note = model.Note,
                        AmountDiscount = model.AmountDiscount,
                        Status = "Đặt Hàng Thành Công",
                        Date = DateTime.UtcNow.AddHours(7),
                        DeliveryStatus = "",
                    };
                    var result = await _orderRepository.AddNewOrder(_mapper.Map<Order>(orderModel));
                    if (result != null)
                    {
                        foreach (var item in model.ProductOrders)
                        {
                            var product = await _productRepositoty.GetProductByIdAsync(item.Id);

                            

                            if (product != null)
                            {
                                product.Quantity -= item.Quantity;

                                await _productRepositoty.UpdateProductAsync();

                                var orderDetailModel = new OrderDetailModel()
                                {
                                    OrderId = result.Id,
                                    Price = product.Price,
                                    ProductId = product.Id,
                                    Quantity = item.Quantity,
                                };
                                await _orderRepository.AddNewOrderDetails(_mapper.Map<OrderDetail>(orderDetailModel));
                            }
                            else
                            {
                                throw new Exception("Không tìm thấy product có id: " + item.Id);
                            }
                        }
                    }
                    else
                    {
                        throw new Exception("Không thể tạo order");
                    }
                    await transaction.CommitAsync();
                    return true;
                }
                catch (Exception ex) 
                {
                    await transaction.RollbackAsync();
                    throw ex;
                }
            }
        }
    }
}
