using System;
using System.Collections.Generic;

namespace NET1806_LittleJoy.Repository.Entities;

public partial class Delivery
{
    public int OrderId { get; set; }

    public string? Status { get; set; }

    public virtual Order? Order { get; set; }
}
