namespace BackAPI.DTO
{
    public class IngredientStringDTO
    {
        public int IngredientStringId { get; set; }

        public int IngredientFk { get; set; }
        
        public string? IngredientName { get; set; }

        public int DishFk { get; set; }
    }
}
