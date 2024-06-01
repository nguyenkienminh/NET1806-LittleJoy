using System;
using System.Collections.Generic;

namespace NET1806_LittleJoy.Repository.Entities;

public partial class AgeGroupProduct
{
    public int Id { get; set; }

    public string? AgeRange { get; set; }

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
