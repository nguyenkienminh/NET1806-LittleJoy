using AutoMapper;
using Microsoft.AspNetCore.Http;
using NET1806_LittleJoy.API.ViewModels.RequestModels;
using NET1806_LittleJoy.Repository.Commons;
using NET1806_LittleJoy.Repository.Entities;
using NET1806_LittleJoy.Repository.Repositories;
using NET1806_LittleJoy.Repository.Repositories.Interface;
using NET1806_LittleJoy.Service.BusinessModels;
using NET1806_LittleJoy.Service.Services.Interface;
using NET1806_LittleJoy.Service.Ultils;
using Org.BouncyCastle.Crypto.Engines;
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
        private readonly IPaymentRepository _paymentRepository;
        private readonly IVNPayService _vnpayservice;
        private readonly IPointsMoneyRepository _pointsMoneyRepository;
        private readonly IMapper _mapper;

        public OrderService(IOrderRepository orderRepository, IProductRepositoty productRepositoty, IUserRepository userRepository, IPaymentRepository paymentRepository, IVNPayService vnpayservice, IMapper mapper)
        {
            _orderRepository = orderRepository;
            _productRepositoty = productRepositoty;
            _userRepository = userRepository;
            _paymentRepository = paymentRepository;
            _vnpayservice = vnpayservice;
            _mapper = mapper;
        }

        public async Task<OrderResponseModel> CreateOrder(OrderRequestModel model, HttpContext context)
        {
            //dùng transaction
            using (var transaction = await _orderRepository.BeginTransactionAsync())
            {
                try
                {
                    //kiểm tra user
                    var user = await _userRepository.GetUserByIdAsync(model.UserId);
                    if (user == null)
                    {
                        throw new Exception("Không tìm thấy user");
                    }

                    //tạo order add vào database
                    var orderModel = new OrderModel()
                    {
                        UserId = user.Id,
                        TotalPrice = model.TotalPrice,
                        Address = model.Address,
                        Note = model.Note,
                        AmountDiscount = model.AmountDiscount,
                        Status = "Đang Chờ",
                        Date = DateTime.UtcNow.AddHours(7),
                        DeliveryStatus = "",
                    };
                    var result = await _orderRepository.AddNewOrder(_mapper.Map<Order>(orderModel));

                    //kiểm tra add order
                    if (result != null)
                    {
                        //tạo orderCode để add vào payment
                        var orderCode = 0;
                        while (true)
                        {
                            orderCode = NumberUltils.GenerateNumber(6);
                            var checkOrderCode = await _paymentRepository.GetPaymentByOrderCode(orderCode);
                            if (checkOrderCode == null)
                            {
                                break;
                            }
                        }

                        //add orderCode
                        string method = "";
                        string urlPayment = "";
                        if (model.PaymentMethod == 1)
                        {
                            method = "COD";
                        } else if (model.PaymentMethod == 2)
                        {
                            method = "VNPAY";
                            urlPayment = _vnpayservice.RequestVNPay(orderCode, model.TotalPrice, context);
                        }
                        else
                        {
                            throw new Exception("Vui lòng chọn đúng payment method");
                        }

                        var payment = new PaymentModel()
                        {
                            OrderID = result.Id,
                            Code = orderCode,
                            Method = method,
                            Status = "Đang chờ",
                        };
                        await _paymentRepository.CreateNewPayment(_mapper.Map<Payment>(payment));

                        //add orderdetails
                        foreach (var item in model.ProductOrders)
                        {
                            var product = await _productRepositoty.GetProductByIdAsync(item.Id);

                            //kiểm tra product
                            if (product != null)
                            {
                                product.Quantity -= item.Quantity;

                                await _productRepositoty.UpdateProductAsync(product);

                                var orderDetailModel = new OrderDetailModel()
                                {
                                    OrderId = result.Id,
                                    Price = product.Price * item.Quantity,
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
                        await transaction.CommitAsync();

                        var response = new OrderResponseModel()
                        {
                            OrderCode = orderCode,
                            UrlPayment = urlPayment,
                            Message = "Đơn hàng được tạo thành công",
                        };
                        return response;
                    }
                    else
                    {
                        throw new Exception("Không thể tạo order");
                    }
                }
                catch (Exception ex) 
                {
                    await transaction.RollbackAsync();
                    throw ex;
                }
            }
        }

        public async Task<Pagination<OrderWithDetailsModel>> GetOrderByUserId(PaginationParameter parameter, int userId)
        {
            //lấy list order
            var list = await _orderRepository.GetOrderByUserId(parameter, userId);

            //chuyen thanh list order with details
            var result = list.Select(x => new OrderWithDetailsModel()
            {
                Id = x.Id,
                Address = x.Address,
                AmountDiscount = x.AmountDiscount,
                Note = x.Note,
                TotalPrice = (int)x.TotalPrice,
                UserId = userId,
                DeliveryStatus = x.DeliveryStatus,
                Status = x.Status
            }).ToList();

            //lay tung order gan them thanh phan
            foreach (var item in result) 
            {
                //lay payment gan vao order
                var payment = await _paymentRepository.GetPaymentByOrderId(item.Id);
                item.PaymentMethod = payment.Method;
                item.PaymentStatus = payment.Status;
                item.OrderCode = (int)payment.Code;

                //gan order details vo
                var listDetails = await _orderRepository.GetOrderDetailsByOrderId(item.Id);
                List<OrderProductModel> listProductDetails = new List<OrderProductModel>();
                foreach (var item1 in listDetails)
                {
                    var product = await _productRepositoty.GetProductByIdAsync((int)item1.ProductId);
                    listProductDetails.Add(new OrderProductModel()
                    {
                        Id = product.Id,
                        ProductName = product.ProductName,
                        Price = (int)item1.Price,
                        Quantity = (int)item1.Quantity,
                    });
                }
                item.ProductOrders = listProductDetails;
            }

            return new Pagination<OrderWithDetailsModel>(result, list.TotalCount, list.CurrentPage, list.PageSize);
        }
    }
}
