using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NET1806_LittleJoy.Service.BusinessModels
{
    public class AddPasswordModel
    {
        [Display(Name = "Email")]
        public string Email { get; set; } = "";

        [Display(Name = "Password")]
        [DataType(DataType.Password)]
        [StringLength(12, MinimumLength = 7, ErrorMessage = "Mật khẩu từ 7-12 kí tự")]
        [PasswordPropertyText]
        public string Password { get; set; } = "";

        [Compare("Password", ErrorMessage = "Mật khẩu không trùng khớp")]
        [Display(Name = "Confirm Password")]
        public string ConfirmPassword { get; set; } = "";
    }


    public class ChangePasswordModel
    {
        public int Id { get; set; }

        public string? OldPassword { get; set; } = "";

        public string? NewPassword { get; set; } = "";

        public string? ConfirmPassword { get; set; } = "";
    }
}
