using System;
using System.Collections.Generic;

namespace BackAPI.Models1;

public partial class DishOrder
{
    public int DishOrderId { get; set; }

    public int DishFk { get; set; }

    public int OrderFk { get; set; }

    public int Number { get; set; }

    public virtual Dish DishFkNavigation { get; set; } = null!;

    public virtual Order OrderFkNavigation { get; set; } = null!;
}
