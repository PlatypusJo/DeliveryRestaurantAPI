namespace BackAPI.DTO
{
    public class DishDTO
    {
        public int DishId { get; set; }

        public string DishName { get; set; } = null!;

        public string DishGrammers { get; set; } = null!;

        public string? CategoryName { get; set; }

        public int CategoryFk { get; set; }

        public string DishCost { get; set; } = null!;

        public string? DishImage { get; set; }

    }
}
