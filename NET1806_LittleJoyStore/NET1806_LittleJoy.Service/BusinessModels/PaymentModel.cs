using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NET1806_LittleJoy.Service.BusinessModels
{
    public class PaymentModel
    {
        public int Id { get; set; }

        public int OrderID { get; set; }

        public string? Status { get; set; }

        public string? Method { get; set; }

        public int? Code { get; set; }
    }
}
