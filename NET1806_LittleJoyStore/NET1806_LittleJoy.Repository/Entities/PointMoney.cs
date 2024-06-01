using System;
using System.Collections.Generic;

namespace NET1806_LittleJoy.Repository.Entities;

public partial class PointMoney
{
    public int Id { get; set; }

    public int? MinPoints { get; set; }

    public int? AmountDiscount { get; set; }
}
