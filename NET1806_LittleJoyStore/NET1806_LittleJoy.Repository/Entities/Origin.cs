using System;
using System.Collections.Generic;

namespace NET1806_LittleJoy.Repository.Entities;

public partial class Origin
{
    public int Id { get; set; }

    public string? OriginName { get; set; }

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
