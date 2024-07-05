using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NET1806_LittleJoy.Repository.Commons
{
    public class UserFilterModel
    {
        public int? RoleId { get; set; }

        public int? status { get; set; }

        public string? userName { get; set; }

        public string? fullName { get; set; }
    }
}
