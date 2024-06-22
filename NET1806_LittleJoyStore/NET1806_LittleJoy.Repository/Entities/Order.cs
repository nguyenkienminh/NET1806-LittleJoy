using System;
using System.Collections.Generic;

namespace NET1806_LittleJoy.Repository.Entities;

public partial class Order
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public int? TotalPrice { get; set; }

    public string Address { get; set; }

    public string? Note { get; set; }

    public int? AmountDiscount { get; set; }

    public string? Status { get; set; }

    public DateTime? Date { get; set; }

    public string? DeliveryStatus { get; set; }

    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();

    public virtual Payment? Payment { get; set; }

    public virtual Refund? Refund { get; set; }

    public virtual User User { get; set; } = null!;
}
