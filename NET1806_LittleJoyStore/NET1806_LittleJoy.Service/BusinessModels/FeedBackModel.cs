using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NET1806_LittleJoy.Service.BusinessModels
{
    public class FeedBackModel
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public int ProductId { get; set; }

        public string? Comment { get; set; }

        public int? Rating { get; set; }

        public DateTime? Date { get; set; }
    }


    public class ProductAverageRating
    {
        public int ProductId { get; set; }

        public double? RatingAver { get; set; }
    }
}
