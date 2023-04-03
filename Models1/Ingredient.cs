using System;
using System.Collections.Generic;

namespace BackAPI.Models1;

public partial class Ingredient
{
    public int IngredientId { get; set; }

    public string IngredientName { get; set; } = null!;

    public virtual ICollection<IngredientString> IngredientStrings { get; } = new List<IngredientString>();
}
