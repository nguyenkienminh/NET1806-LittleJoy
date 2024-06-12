using System;
using System.Collections.Generic;

namespace NET1806_LittleJoy.Repository.Entities;

public partial class User
{
    public int Id { get; set; }

    public string? UserName { get; set; }

    public string? PasswordHash { get; set; }

    public int? RoleId { get; set; }

    public string? Fullname { get; set; }

    public string Email { get; set; } = null!;

    public string? Avatar { get; set; }

    public string? GoogleId { get; set; }

    public string? PhoneNumber { get; set; }

    public bool? Status { get; set; }

    public int? Points { get; set; }

    public string? UnsignName { get; set; }

    public bool ConfirmEmail  { get; set; }

    public string? TokenConfirmEmail { get; set; }

    public virtual ICollection<Address> Addresses { get; set; } = new List<Address>();

    public virtual ICollection<Feedback> Feedbacks { get; set; } = new List<Feedback>();

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    public virtual ICollection<Post> Posts { get; set; } = new List<Post>();

    public virtual Role? Role { get; set; }
}
