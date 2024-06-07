using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NET1806_LittleJoy.Repository.Commons
{
    public class BlogFilterModel
    {
        public string? search { get; set; }
        public int? sortDate { get; set; }
        public int? UserId { get; set; }
    }
}
