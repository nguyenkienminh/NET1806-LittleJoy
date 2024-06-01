using System;
using System.Collections.Generic;

namespace NET1806_LittleJoy.Repository.Entities;

public partial class Payment
{
    public int Id { get; set; }

    public int? OrderId { get; set; }

    public string? Status { get; set; }

    public string? Method { get; set; }

    public int? Code { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
