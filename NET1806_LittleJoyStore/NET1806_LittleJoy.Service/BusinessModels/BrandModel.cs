using NET1806_LittleJoy.Repository.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NET1806_LittleJoy.Service.BusinessModels
{
    public class BrandModel
    {
        public int Id { get; set; }

        public string? BrandName { get; set; }

        public string? Logo { get; set; }

        public string? BrandDescription { get; set; }

    }
}
