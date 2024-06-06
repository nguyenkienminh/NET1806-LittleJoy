using NET1806_LittleJoy.Repository.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NET1806_LittleJoy.Service.BusinessModels
{
    public class BlogModel
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public string? Title { get; set; }

        public string? Banner { get; set; }

        public string? Content { get; set; }

        public DateTime? Date { get; set; }

        public string? UnsignTitle { get; set; }
    }
}
