using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using NET1806_LittleJoy.Repository.Commons;
using NET1806_LittleJoy.Repository.Repositories;
using NET1806_LittleJoy.Repository.Repositories.Interface;
using NET1806_LittleJoy.Service.BusinessModels;
using NET1806_LittleJoy.Service.Helpers;
using NET1806_LittleJoy.Service.Services.Interface;
using NET1806_LittleJoy.Service.Ultils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace NET1806_LittleJoy.Service.Services
{
    public class VNPayService : IVNPayService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IProductRepositoty _productRepositoty;
        private readonly IUserRepository _userRepository;
        private readonly IPaymentRepository _paymentRepository;
        private readonly IPointsMoneyRepository _pointsMoneyRepository;
        private readonly IMailService _mailService;
        private readonly IMapper _mapper;

        public VNPayService(IOrderRepository orderRepository, IUserRepository userRepository, IPointsMoneyRepository pointsMoneyRepository, IPaymentRepository paymentRepository, IMailService mailService, IProductRepositoty productRepositoty, IMapper mapper) 
        {
            _orderRepository = orderRepository;
            _userRepository = userRepository;
            _paymentRepository = paymentRepository;
            _pointsMoneyRepository = pointsMoneyRepository;
            _mailService = mailService;
            _productRepositoty = productRepositoty;
            _mapper = mapper;
        }
        public string RequestVNPay(int orderCode, int price, HttpContext context)
        {
            IConfiguration _configuration = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json")
                    .Build();

            DateTime dateTime = DateTime.UtcNow.AddHours(7);

            var ipAddress = VnPayUtils.GetIpAddress(context);

            var pay = new VnPayLibrary();

            pay.AddRequestData("vnp_Version", _configuration["Vnpay:Version"]);
            pay.AddRequestData("vnp_Command", _configuration["Vnpay:Command"]);
            pay.AddRequestData("vnp_TmnCode", _configuration["Vnpay:TmnCode"]);
            pay.AddRequestData("vnp_Amount", (price * 100).ToString());
            pay.AddRequestData("vnp_CreateDate", dateTime.ToString("yyyyMMddHHmmss"));
            pay.AddRequestData("vnp_CurrCode", _configuration["Vnpay:CurrCode"]);
            pay.AddRequestData("vnp_IpAddr", ipAddress);
            pay.AddRequestData("vnp_Locale", _configuration["Vnpay:Locale"]);
            pay.AddRequestData("vnp_OrderInfo", "Thanh toan cho don hang: " + orderCode);
            pay.AddRequestData("vnp_OrderType", "250000");
            pay.AddRequestData("vnp_TxnRef", orderCode.ToString());
            pay.AddRequestData("vnp_ReturnUrl", _configuration["Vnpay:UrlReturn"]);

            var paymentUrl =
                pay.CreateRequestUrl(_configuration["Vnpay:BaseUrl"], _configuration["Vnpay:HashSecret"]);

            return paymentUrl;
        }

        public async Task<PaymentModel> ReturnFromVNPay(VNPayModel vnPayResponse)
        {
            IConfiguration _configuration = new ConfigurationBuilder()
                        .SetBasePath(Directory.GetCurrentDirectory())
                        .AddJsonFile("appsettings.json")
                        .Build();

            if (vnPayResponse != null)
            {
                var vnpay = new VnPayLibrary();

                foreach (PropertyInfo prop in vnPayResponse.GetType().GetProperties())
                {
                    string name = prop.Name;
                    object value = prop.GetValue(vnPayResponse, null);
                    string valueStr = value?.ToString() ?? string.Empty;
                    vnpay.AddResponseData(name, valueStr);
                }

                var vnpayHashSecret = _configuration["Vnpay:HashSecret"];
                bool validateSignature = vnpay.ValidateSignature(vnPayResponse.vnp_SecureHash, vnpayHashSecret);

                if (validateSignature)
                {
                    //lấy ordercode từ vnpay response
                    int orderCode = 0;
                    _ = int.TryParse(vnPayResponse.vnp_TxnRef, out orderCode);

                    //lấy payment từ ordercode
                    var payment = await _paymentRepository.GetPaymentByOrderCode(orderCode);
                    var order = await _orderRepository.GetOrderById(payment.OrderID);
                    if (vnPayResponse.vnp_TransactionStatus == "00")
                    {
                        //cập nhật tình trạng thanh toán
                        payment.Status = "Thành Công";
                        var result = await _paymentRepository.UpdatePayment(payment);

                        //update trạng thái order
                        
                        order.Status = "Đặt Hàng Thành Công";
                        await _orderRepository.UpdateOrder(order);

                        //lấy user để cộng, trừ điểm theo order
                        var user = await _userRepository.GetUserByIdAsync(order.UserId);
                        if(order.AmountDiscount != 0)
                        {
                            //nếu có dùng điểm thì trừ điểm
                            var points = await _pointsMoneyRepository.GetPointsByMoneyDiscount(order.AmountDiscount);
                            user.Points -= points.MinPoints;
                        }

                        //cộng điểm theo đơn hàng
                        user.Points += order.TotalPrice / 1000;
                        
                        //update user
                        await _userRepository.UpdateUserAsync(user);

                        //send mail
                        var orderWithDetails = await GetOrderWithDetailsAsync(orderCode);
                        string body = EmailContent.OrderEmail(orderWithDetails, _mapper.Map<UserModel>(user));

                        await _mailService.sendEmailAsync(new MailRequest()
                        {
                            ToEmail = user.Email,
                            Body = body,
                            Subject = "[Little Joy] Hóa đơn điện tử số #"+orderCode
                        });

                        return _mapper.Map<PaymentModel>(result);
                    }
                    else
                    {
                        //update payment
                        payment.Status = "Thất Bại";
                        var result = await _paymentRepository.UpdatePayment(payment);

                        //update order
                        order.Status = "Đã Hủy";
                        await _orderRepository.UpdateOrder(order);
                        return _mapper.Map<PaymentModel>(result);
                    }
                }
                else
                {
                    throw new Exception("Không đúng signature");
                }
            }
            else
            {
                throw new Exception("Có lỗi trong quá trình return");
            }
        }

        private async Task<OrderWithDetailsModel> GetOrderWithDetailsAsync(int orderCode)
        {
            var payment = await _paymentRepository.GetPaymentByOrderCode(orderCode);
            if (payment == null)
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
    }
}
