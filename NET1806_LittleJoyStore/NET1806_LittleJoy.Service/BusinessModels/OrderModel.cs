using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NET1806_LittleJoy.Service.BusinessModels
{
    public class OrderModel
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public int? TotalPrice { get; set; }

        public string Address { get; set; }

        public string? Note { get; set; }

        public int? AmountDiscount { get; set; }

        public string? Status { get; set; }

        public DateTime? Date { get; set; }

        public int? PaymentId { get; set; }

        public string? DeliveryStatus { get; set; }
    }
}
