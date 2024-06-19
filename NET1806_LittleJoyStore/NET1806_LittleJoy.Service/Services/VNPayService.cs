using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using NET1806_LittleJoy.Repository.Repositories.Interface;
using NET1806_LittleJoy.Service.BusinessModels;
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

        public VNPayService(IOrderRepository orderRepository) 
        {
            _orderRepository = orderRepository;
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

        public async Task<bool> ReturnFromVNPay(VNPayModel vnPayResponse)
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
                    if(vnPayResponse.vnp_TransactionStatus == "00")
                    {
                        //làm gì đó đúng trong đây
                        return true;
                    }
                    else
                    {
                        //làm gì đó sai trong đây
                        return false;
                    }
                }
            }
            return false;
        }
    }
}
