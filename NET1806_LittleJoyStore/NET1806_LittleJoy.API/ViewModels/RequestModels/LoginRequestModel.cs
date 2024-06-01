using System.ComponentModel.DataAnnotations;

namespace NET1806_LittleJoy.API.ViewModels.RequestModels
{
    public class LoginRequestModel
    {
        [Required(ErrorMessage = "Username is required")]
        public string Username { get; set; } = "";
        public string Password { get; set; } = "";
    }
}
