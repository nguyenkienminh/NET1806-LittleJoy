using System.ComponentModel.DataAnnotations;

namespace NET1806_LittleJoy.API.ViewModels.RequestModels
{
    public class OtpRequestModel
    {
        [Required(ErrorMessage = "Email is required")]
        public string Email { get; set; } = "";

        [Required(ErrorMessage = "OTP is required")]
        [Display(Name = "OTP")]
        public int OTPCode { get; set; } = 0;
    }
}
