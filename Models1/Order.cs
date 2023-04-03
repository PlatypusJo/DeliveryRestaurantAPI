using System;
using System.Collections.Generic;

namespace BackAPI.Models1;

public partial class Order
{
    public int OrderId { get; set; }

    public int OrderNumber { get; set; }

    public DateTime OrderDate { get; set; }

    public int UserFk { get; set; }

    public double Total { get; set; }

    public virtual ICollection<DishOrder> DishOrders { get; } = new List<DishOrder>();

}
