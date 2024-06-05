using System.ComponentModel.DataAnnotations;

namespace NET1806_LittleJoy.API.ViewModels.RequestModels
{
    public class CategoryRequestModel
    {
        [MaxLength(100)]
        public string? CategoryName { get; set; }
    }
}
