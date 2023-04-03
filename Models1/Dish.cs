using System;
using System.Collections.Generic;

namespace BackAPI.Models1;

public partial class Dish
{
    public int DishId { get; set; }

    public string DishName { get; set; } = null!;

    public int DishGrammers { get; set; }

    public int CategoryFk { get; set; }

    public int DishCost { get; set; }

    public string? DishImage { get; set; }

    public virtual Category CategoryFkNavigation { get; set; } = null!;

    public virtual ICollection<DishOrder> DishOrders { get; } = new List<DishOrder>();

    public virtual ICollection<IngredientString> IngredientStrings { get; } = new List<IngredientString>();
}
