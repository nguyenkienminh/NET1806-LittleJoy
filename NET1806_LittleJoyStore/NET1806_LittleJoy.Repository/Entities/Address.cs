using System;
using System.Collections.Generic;

namespace NET1806_LittleJoy.Repository.Entities;

public partial class Address
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public string? Address1 { get; set; }

    public bool? IsMainAddress { get; set; }

    public virtual User User { get; set; } = null!;
}
