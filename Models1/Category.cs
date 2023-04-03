using System;
using System.Collections.Generic;

namespace BackAPI.Models1;

public partial class Category
{
    public int CategoryId { get; set; }

    public string CategoryName { get; set; } = null!;

    public virtual ICollection<Dish> Dishes { get; } = new List<Dish>();
}
