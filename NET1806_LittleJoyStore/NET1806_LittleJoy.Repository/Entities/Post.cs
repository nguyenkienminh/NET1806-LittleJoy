using System;
using System.Collections.Generic;

namespace NET1806_LittleJoy.Repository.Entities;

public partial class Post
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public string? Title { get; set; }

    public string? Banner { get; set; }

    public string? Content { get; set; }

    public DateTime? Date { get; set; }

    public string? UnsignTitle { get; set; }

    public virtual User User { get; set; } = null!;
}
