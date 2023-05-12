using BackAPI.Models1;

namespace BackAPI.DTO
{
    /// <summary>
    /// DTO класс, реализующий связь между клиентской и серверной частью. Служит для отображения информации о блюде на странице
    /// </summary>
    public class DishDTO
    {
        public int DishId { get; set; }

        public string DishName { get; set; } = null!;

        public int DishGrammers { get; set; }
        /// <summary>
        /// Используется для хранения названия категории
        /// </summary>
        public string? CategoryName { get; set; }

        public int CategoryFk { get; set; }

        public int DishCost { get; set; }

        public string? DishImage { get; set; }
        /// <summary>
        /// Список ингредиентов блюда (DTO класс)
        /// </summary>
        public virtual ICollection<IngredientStringDTO> IngredientStringsDTO { get; set; } = new List<IngredientStringDTO>();

    }
}
