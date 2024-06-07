using System.ComponentModel.DataAnnotations;

namespace NET1806_LittleJoy.API.ViewModels.RequestModels
{
    public class AddressRequestModel
    {
        public int UserId { get; set; }

        [MaxLength]
        public string? Address1 { get; set; }
    }
}
