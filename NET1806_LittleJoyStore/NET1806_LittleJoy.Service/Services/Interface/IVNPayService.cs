using Microsoft.AspNetCore.Http;
using NET1806_LittleJoy.Service.BusinessModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NET1806_LittleJoy.Service.Services.Interface
{
    public interface IVNPayService
    {
        public string RequestVNPay(int orderCode, int price, HttpContext context);

        public Task<PaymentModel> ReturnFromVNPay(VNPayModel vnPayResponse);
    }
}
