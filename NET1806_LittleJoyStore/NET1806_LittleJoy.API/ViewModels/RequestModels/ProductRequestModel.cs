using System.ComponentModel.DataAnnotations;

namespace NET1806_LittleJoy.API.ViewModels.RequestModels
{
    public class ProductRequestModel
    {

        [MaxLength]
        public string? ProductName { get; set; }

        public int? Price { get; set; }

        [MaxLength]
        public string? Description { get; set; }

        public int? Weight { get; set; }

        public int Quantity { get; set; }

        public string? Image { get; set; }

        public bool? IsActive { get; set; }

        /*************************************************/

        public int? AgeId { get; set; }

        public int? OriginId { get; set; }

        public int? BrandId { get; set; }

        public int? CateId { get; set; }

    }
}
