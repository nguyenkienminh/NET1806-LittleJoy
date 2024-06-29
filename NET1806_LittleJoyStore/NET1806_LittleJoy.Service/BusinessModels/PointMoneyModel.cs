using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NET1806_LittleJoy.Service.BusinessModels
{
    public class PointMoneyModel
    {
        public int Id { get; set; }

        public int? MinPoints { get; set; }

        public int? AmountDiscount { get; set; }
    }
}
