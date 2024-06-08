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
        public int Id { get; set; }

        public string? UserName { get; set; }

        public int? RoleId { get; set; }

        public string? Fullname { get; set; }

        public string Email { get; set; } = null!;

        public string? Avatar { get; set; }

        public string? GoogleId { get; set; }

        public string? PhoneNumber { get; set; }

        public bool? Status { get; set; }

        public int? Points { get; set; }

        public string? UnsignName { get; set; }

        public bool ConfirmEmail { get; set; }
    }
}
