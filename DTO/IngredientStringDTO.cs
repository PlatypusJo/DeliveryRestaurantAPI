namespace BackAPI.DTO
{
    /// <summary>
    /// DTO класс, реализующий связь между клиентской и серверной частью. Служит для отображения информации об ингредиентах блюда на странице
    /// </summary>
    public class IngredientStringDTO
    {
        public int IngredientStringId { get; set; }

        public int IngredientFk { get; set; }
        /// <summary>
        /// Используется для хранения и отображения названия ингредиента
        /// </summary>
        public string? IngredientName { get; set; }

        public int DishFk { get; set; }
    }
}
