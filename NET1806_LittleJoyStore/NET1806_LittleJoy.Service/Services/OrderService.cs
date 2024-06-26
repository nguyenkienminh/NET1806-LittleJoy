using AutoMapper;
using Microsoft.AspNetCore.Http;
using NET1806_LittleJoy.API.ViewModels.RequestModels;
using NET1806_LittleJoy.Repository.Commons;
using NET1806_LittleJoy.Repository.Entities;
using NET1806_LittleJoy.Repository.Repositories;
using NET1806_LittleJoy.Repository.Repositories.Interface;
using NET1806_LittleJoy.Service.BusinessModels;
using NET1806_LittleJoy.Service.Helpers;
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
        private readonly IMailService _mailService;
        private readonly IPointsMoneyRepository _pointsMoneyRepository;
        private readonly IMapper _mapper;

        public OrderService(IOrderRepository orderRepository, IProductRepositoty productRepositoty, IUserRepository userRepository, IPaymentRepository paymentRepository, IVNPayService vnpayservice, IPointsMoneyRepository pointsMoneyRepository, IMailService mailService, IMapper mapper)
        {
            _orderRepository = orderRepository;
            _productRepositoty = productRepositoty;
            _userRepository = userRepository;
            _paymentRepository = paymentRepository;
            _vnpayservice = vnpayservice;
            _pointsMoneyRepository = pointsMoneyRepository;
            _mailService = mailService;
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
                            result.Status = "Đặt Hàng Thành Công";
                            await _orderRepository.UpdateOrder(result);
                        }
                        else if (model.PaymentMethod == 2)
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

        public async Task<OrderWithDetailsModel> GetOrderByOrderCode(int orderCode)
        {
            var payment = await _paymentRepository.GetPaymentByOrderCode(orderCode);
            if(payment == null)
            {
                throw new Exception("Không tìm thấy order");
            }
            var order = await _orderRepository.GetOrderById(payment.OrderID);

            OrderWithDetailsModel model = new OrderWithDetailsModel()
            {
                Id = order.Id,
                Address = order.Address,
                AmountDiscount = order.AmountDiscount,
                DeliveryStatus = order.DeliveryStatus,
                Note = order.Note,
                OrderCode = orderCode,
                PaymentMethod = payment.Method,
                PaymentStatus = payment.Status,
                Status = order.Status,
                TotalPrice = (int)order.TotalPrice,
                UserId = order.UserId,
                date = (DateTime)order.Date,
            };

            var listDetails = await _orderRepository.GetOrderDetailsByOrderId(order.Id);
            List<OrderProductModel> listProductDetails = new List<OrderProductModel>();
            foreach (var item1 in listDetails)
            {
                var product = await _productRepositoty.GetProductByIdAsync((int)item1.ProductId);
                listProductDetails.Add(new OrderProductModel()
                {
                    Id = product.Id,
                    ProductName = product.ProductName,
                    Price = ((int)(item1.Price * item1.Quantity)),
                    Quantity = (int)item1.Quantity,
                });
            }
            model.ProductOrders = listProductDetails;
            return model;
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
                Status = x.Status,
                date = (DateTime)x.Date
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
                        Price = ((int)(item1.Price * item1.Quantity)),
                        Quantity = (int)item1.Quantity,
                    });
                }
                item.ProductOrders = listProductDetails;
            }

            return new Pagination<OrderWithDetailsModel>(result, list.TotalCount, list.CurrentPage, list.PageSize);
        }

        public async Task<bool> UpdateOrderDelivery(OrderUpdateRequestModel model)
        {
            var paymentExist = await _paymentRepository.GetPaymentByOrderCode(model.OrderCode);
            var orderExist = await _orderRepository.GetOrderById(paymentExist.OrderID);

            //cập nhật tình trạng giao hàng, thanh toán
            if (orderExist.DeliveryStatus != "Giao Hàng Thành Công" && orderExist.Status == "Đặt Hàng Thành Công")
            {
                switch (model.Status)
                {
                    case 1:
                        {
                            orderExist.DeliveryStatus = "Đang Chuẩn Bị";
                            break;
                        }
                    case 2:
                        {
                            orderExist.DeliveryStatus = "Đang Giao Hàng";
                            break;
                        }
                    case 3:
                        {
                            orderExist.DeliveryStatus = "Giao Hàng Thất Bại";
                            if(paymentExist.Method == "COD")
                            {
                                paymentExist.Status = "Thất Bại";
                                await _paymentRepository.UpdatePayment(paymentExist);
                            }
                            break;
                        }
                    case 4:
                        {
                            orderExist.DeliveryStatus = "Giao Hàng Thành Công";
                            if (paymentExist.Method == "COD")
                            {
                                paymentExist.Status = "Thành Công";
                                await _paymentRepository.UpdatePayment(paymentExist);

                                var user = await _userRepository.GetUserByIdAsync(orderExist.UserId);
                                if (orderExist.AmountDiscount != 0)
                                {
                                    //nếu có dùng điểm thì trừ điểm
                                    var points = await _pointsMoneyRepository.GetPointsByMoneyDiscount(orderExist.AmountDiscount);
                                    user.Points -= points.MinPoints;
                                }

                                //cộng điểm theo đơn hàng
                                user.Points += orderExist.TotalPrice / 1000;

                                //update user
                                await _userRepository.UpdateUserAsync(user);

                                var orderWithDetails = await GetOrderByOrderCode((int)paymentExist.Code);
                                string body = EmailContent.OrderEmail(orderWithDetails, _mapper.Map<UserModel>(user));

                                await _mailService.sendEmailAsync(new MailRequest()
                                {
                                    ToEmail = user.Email,
                                    Body = body,
                                    Subject = "[Little Joy] Hóa đơn điện tử số #" + paymentExist.Code
                                });
                            }
                            break;
                        }
                    default:
                        {
                            throw new Exception("Vui Lòng Chọn Đúng Trạng Thái 1-4");
                        }
                }
                await _orderRepository.UpdateOrder(orderExist);
                return true;
            }
            else
            {
                throw new Exception("Không Thể Cập Nhật Đơn Hàng");
            }
        }

        public async Task<bool> UpdateOrderStatus(OrderUpdateRequestModel model)
        {
            var paymentExist = await _paymentRepository.GetPaymentByOrderCode(model.OrderCode);
            var orderExist = await _orderRepository.GetOrderById(paymentExist.OrderID);

            if (orderExist.DeliveryStatus != "Đang Giao Hàng" && orderExist.DeliveryStatus != "Giao Hàng Thành Công" && orderExist.DeliveryStatus != "Đang Chuẩn Bị")
            {
                if(paymentExist.Method == "VNPay" && paymentExist.Status == "Thành Công")
                {
                    throw new Exception("Không thể cập nhật đơn hàng này");
                }

                string status = "";

                switch (model.Status) 
                {
                    case 1:
                        {
                            status = "Đặt Hàng Thành Công";
                            break;
                        }
                    case 2: 
                        {
                            status = "Đã Hủy";
                            break;
                        }
                }

                if (orderExist.Status == status)
                {
                    throw new Exception("Không Có Gì Để Cập Nhật");
                }
                else 
                {
                    orderExist.Status = status;
                    await _orderRepository.UpdateOrder(orderExist);
                }
                return true;
            }
            else
            {
                throw new Exception("Không thể cập nhật đơn hàng này");
            }
        }
    }
}
