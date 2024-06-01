using System;
using System.Collections.Generic;

namespace NET1806_LittleJoy.Repository.Entities;

public partial class Brand
{
    public int Id { get; set; }

    public string? BrandName { get; set; }

    public string? Logo { get; set; }

    public string? BrandDescription { get; set; }

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
