using System.ComponentModel.DataAnnotations;

namespace NET1806_LittleJoy.API.ViewModels.RequestModels
{
    public class OriginRequestModel
    {
        [MaxLength(100)]
        public string? OriginName { get; set; }
    }
}
