using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NET1806_LittleJoy.Repository.Commons
{
    /// <summary>
    /// This is the product filter model.
    /// </summary>
    public class ProductFilterModel
    {
        /// <summary> GG </summary>
        public int? sortOrder { get; set; }

        public string? keyword { get; set; }

        public int? cateId { get; set; }

        public int? ageId { get; set; }

        public int? originId { get; set; }

        public int? brandId { get; set; }

        public bool? IsActive { get; set; }
    }


    public class ProductFilterStatusModel 
    { 
        public int? status { get; set;}
    }

}
