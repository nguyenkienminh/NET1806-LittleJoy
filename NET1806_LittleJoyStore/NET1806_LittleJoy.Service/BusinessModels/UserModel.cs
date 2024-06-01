using NET1806_LittleJoy.Repository.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NET1806_LittleJoy.Service.BusinessModels
{
    public class UserModel
    {
        public string UserName { get; set; }

        public string? PasswordHash { get; set; }

        public string? Fullname { get; set; }

        public string Email { get; set; } = null!;

        public string? PhoneNumber { get; set; }

        public int? Points { get; set; }
    }
}
