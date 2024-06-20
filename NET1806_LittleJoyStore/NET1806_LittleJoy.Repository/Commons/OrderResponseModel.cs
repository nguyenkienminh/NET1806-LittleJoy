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
}
