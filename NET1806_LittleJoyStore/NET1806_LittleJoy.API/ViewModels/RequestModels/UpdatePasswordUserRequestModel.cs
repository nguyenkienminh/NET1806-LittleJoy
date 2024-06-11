using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace NET1806_LittleJoy.API.ViewModels.RequestModels
{
    public class UpdatePasswordUserRequestModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [Display(Name = "OldPassword")]
        [DataType(DataType.Password)]
        [StringLength(12, MinimumLength = 7, ErrorMessage = "Password must be 7-12 Character")]
        [PasswordPropertyText]
        public string? OldPassword { get; set; } = "";


        [Required(ErrorMessage = "Password is required")]
        [Display(Name = "NewPassword")]
        [DataType(DataType.Password)]
        [StringLength(12, MinimumLength = 7, ErrorMessage = "Password must be 7-12 Character")]
        [PasswordPropertyText]
        public string? NewPassword { get; set; } = "";


        [Required(ErrorMessage = "Confirm Password is required.")]
        [Compare("NewPassword", ErrorMessage = "The password and confirmation password do not match.")]
        [Display(Name = "Confirm Password")]
        public string? ConfirmPassword { get; set; } = "";
    }
}
