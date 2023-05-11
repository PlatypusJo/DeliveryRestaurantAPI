using BackAPI.Models1;

namespace BackAPI.DTO
{
    public class DishDTO
    {
        public int DishId { get; set; }

        public string DishName { get; set; } = null!;

        public int DishGrammers { get; set; }

        public string? CategoryName { get; set; }

        public int CategoryFk { get; set; }

        public int DishCost { get; set; }

        public string? DishImage { get; set; }

        public virtual ICollection<IngredientStringDTO> IngredientStringsDTO { get; set; } = new List<IngredientStringDTO>();

    }
}
