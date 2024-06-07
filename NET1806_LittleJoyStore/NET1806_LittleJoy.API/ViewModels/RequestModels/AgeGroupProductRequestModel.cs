using System.ComponentModel.DataAnnotations;

namespace NET1806_LittleJoy.API.ViewModels.RequestModels
{
    public class AgeGroupProductRequestModel
    {
        [MaxLength]
        public string? AgeRange { get; set; }
    }
}
