using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NET1806_LittleJoy.Repository.Commons
{
    public class OrderFilterModel
    {
        public int OrderCode { get; set; }
        public string UserName { get; set; }
        public int Status { get; set; }
        public int DeliveryStatus { get; set; }
        public int SortDate { get; set; }
        public int SortPrice { get; set; }
        public int MethodPayment { get; set; }
        public int MethodStatus { get; set; }
    }
}
