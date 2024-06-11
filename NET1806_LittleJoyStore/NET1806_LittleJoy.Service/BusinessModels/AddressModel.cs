using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NET1806_LittleJoy.Service.BusinessModels
{
    public class AddressModel
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public string? Address1 { get; set; }

        public bool? IsMainAddress { get; set; }
    }
}
