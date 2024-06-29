using NET1806_LittleJoy.API.ViewModels.RequestModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NET1806_LittleJoy.Repository.Commons
{
    public class OrderResponseModel
    {
        public int OrderCode { get; set; }
        public string UrlPayment { get; set; }
        public string Message {  get; set; }
    }

    public class OrderWithDetailsModel
    {
        public int Id { get; set; }

        public int OrderCode { get; set; }

        public int UserId { get; set; }

        public int TotalPrice { get; set; }

        public string Address { get; set; }

        public DateTime date { get; set; }

        public string? Note { get; set; }

        public int? AmountDiscount { get; set; }

        public string PaymentMethod { get; set; }

        public string PaymentStatus { get; set; }

        public string Status { get; set; }

        public string? DeliveryStatus { get; set; }

        public List<OrderProductModel> ProductOrders { get; set; }
    }

    public class OrderProductModel
    {
        public int Id { get; set; }

        public string ProductName { get; set; }

        public int Price { get; set; }

        public int Quantity { get; set; }
    }
}
