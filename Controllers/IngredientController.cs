using Microsoft.AspNetCore.Mvc;
using BackAPI.DTO;
using Microsoft.AspNetCore.Cors;
using BackAPI.Models1;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BackAPI.Controllers
{
    /// <summary>
    /// Контроллер ингредиента для выполнения запросов на сервер
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class IngredientController : ControllerBase
    {
        private readonly RestaurantDeliveryContext _context;
        /// <summary>
        /// Конструктор контроллера ингредиента, получающий в качестве параметра контекст БД
        /// </summary>
        /// <param name="context">Контекст БД</param>
        public IngredientController(RestaurantDeliveryContext context)
        {
            _context = context;
        }
        /// <summary>
        /// Получение всех ингредиентов из БД
        /// </summary>
        /// <returns>Список ингредиентов</returns>
        // GET: api/<IngredientController>
        [HttpGet]
        public async Task<ActionResult<List<Ingredient>>> GetIngredient()
        {
            return await _context.Ingredients.ToListAsync();
        }
        /// <summary>
        /// Получение ингредиента из БД по id
        /// </summary>
        /// <param name="id">id искомого ингредиента</param>
        /// <returns>искомый ингредиент</returns>
        // GET api/<IngredientController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Ingredient>> GetIngredient(int id)
        {
            var ingredient = await _context.Ingredients.FindAsync(id);
            if (ingredient == null)
            {
                return NotFound();
            }
            return Ok(ingredient);
        }
        /// <summary>
        /// Создание нового ингредиента в БД
        /// </summary>
        /// <param name="ingredient">новый ингредиент</param>
        /// <returns>добавленный ингредиент</returns>
        // POST api/<IngredientController>
        [HttpPost]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult<Ingredient>> Post([FromBody] Ingredient ingredient)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            Ingredient ingredient1 = new()
            {
                IngredientName = ingredient.IngredientName
            };
            _context.Ingredients.Add(ingredient);
            await _context.SaveChangesAsync();
            ingredient.IngredientId = ingredient1.IngredientId;
            return CreatedAtAction("GetIngredient", new { id = ingredient.IngredientId }, ingredient);
        }
        /// <summary>
        /// Изменение существующего ингредиента в БД
        /// </summary>
        /// <param name="id">id изменяемого ингредиента</param>
        /// <param name="newIngredient">изменённый ингредиент, содержит обновлённые данные</param>
        /// <returns></returns>
        // PUT api/<IngredientController>/5
        [HttpPut("{id}")]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult<List<Ingredient>>> Put([FromRoute] int id, [FromBody] Ingredient newIngredient)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var ingredient = await _context.Ingredients.FindAsync(id);
            if (ingredient == null)
            {
                return NotFound();
            }
            ingredient.IngredientName = newIngredient.IngredientName;
            _context.Ingredients.Update(ingredient);
            await _context.SaveChangesAsync();
            return await GetIngredient();
        }
        /// <summary>
        /// Удаление ингредиента и связанных с ним сущности по id из БД
        /// </summary>
        /// <param name="id">id удаляемого ингредиента</param>
        /// <returns>Статус выполнения запроса</returns>
        // DELETE api/<IngredientController>/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var ingredient = await _context.Ingredients.FindAsync(id);
            if (ingredient == null)
            {
                return NotFound();
            }
            await foreach(var inStr in _context.IngredientStrings)
            {
                if (ingredient.IngredientId == inStr.IngredientFk)
                {
                    _context.IngredientStrings.Remove(inStr);
                }
            }
            _context.Ingredients.Remove(ingredient);
            await _context.SaveChangesAsync();
            return Ok("Сущность успешно удалена");
        }
    }
}
