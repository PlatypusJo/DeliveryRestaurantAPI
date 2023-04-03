using System;
using System.Collections.Generic;

namespace BackAPI.Models1;

public partial class IngredientString
{
    public int IngredientStringId { get; set; }

    public int IngredientFk { get; set; }

    public int DishFk { get; set; }

    public virtual Dish DishFkNavigation { get; set; } = null!;

    public virtual Ingredient IngredientFkNavigation { get; set; } = null!;
}
