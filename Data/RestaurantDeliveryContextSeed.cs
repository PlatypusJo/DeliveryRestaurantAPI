using BackAPI.Models1;

namespace BackAPI.Data
{
    public class RestaurantDeliveryContextSeed
    {
        /// <summary>
        /// Инициализирует передаваемый контекст БД значениями по умолчанию если в нём нет данных
        /// </summary>
        /// <param name="context">Контекст БД</param>
        /// <returns></returns>
        public static async Task Initialize(RestaurantDeliveryContext context)
        {
            try
            {
                context.Database.EnsureCreated();
                if (!context.Categories.Any())
                {
                    var categories = new Category[]
                    {
                        new Category { CategoryName = "Супы"},
                        new Category { CategoryName = "Закуски" }
                    };

                    foreach (Category category in categories)
                    {
                        context.Categories.Add(category);
                    }
                    await context.SaveChangesAsync();
                }


                if (!context.Ingredients.Any())
                {
                    var ingredients = new Ingredient[]
                    {
                        new Ingredient { IngredientName = "Морковь" },
                        new Ingredient {  IngredientName = "Лук" },
                        new Ingredient { IngredientName = "Картофель"},
                        new Ingredient { IngredientName = "Чеснок"}
                    };

                    foreach (Ingredient ingredient in ingredients)
                    {
                        context.Ingredients.Add(ingredient);
                    }
                    await context.SaveChangesAsync();
                }


                if (!context.Dishes.Any())
                {
                    var dishes = new Dish[]
                    {
                        new Dish
                        {
                            DishName = "Морковный суп с сыром",
                            CategoryFk = 1,
                            DishCost = 450,
                            DishGrammers = 300,
                            DishImage = "https://de-fragrance.ru/wp-content/uploads/e/7/d/e7df20e81f80292b40e405393aae7867.jpeg"
                        },
                        new Dish
                        {
                            DishName = "Рыбный суп с рисом",
                            CategoryFk = 1,
                            DishCost = 300,
                            DishGrammers = 350,
                            DishImage = "https://avatars.mds.yandex.net/i?id=89a8829b415ad1fc2e8e3b29b7aac263dfc7b8d1-7950464-images-thumbs&n=13&exp=1"
                        },
                        new Dish
                        {
                            DishName = "Картошка по-деревенски с чесночным соусом",
                            CategoryFk = 2,
                            DishCost = 400,
                            DishGrammers = 300,
                            DishImage = "https://pic.rutubelist.ru/video/7e/b9/7eb9e2dcfd6bb131d18726819780b989.jpg"
                        }
                    };

                    foreach (Dish dish in dishes)
                    {
                        context.Dishes.Add(dish);
                    }
                    await context.SaveChangesAsync();
                }

            }
            catch
            {
                throw;
            }
        }
    }
}
