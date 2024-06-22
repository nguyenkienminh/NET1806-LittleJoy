using System;
using System.Collections.Generic;

namespace NET1806_LittleJoy.Repository.Entities;

public partial class Payment
{
    public int Id { get; set; }

    public int OrderID { get; set; }

    public string? Status { get; set; }

    public string? Method { get; set; }

    public int? Code { get; set; }

    public virtual Order Order { get; set; }
}
