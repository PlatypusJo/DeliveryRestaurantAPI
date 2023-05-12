using Microsoft.AspNetCore.Mvc;
using BackAPI.DTO;
using BackAPI.Models1;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using System.Data;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BackAPI.Controllers
{

    /// <summary>
    /// Контроллер строки ингредиента для выполнения запросов на сервер
    /// </summary>

    [Route("api/[controller]")]
    [ApiController]

    public class IngredientStringController : ControllerBase
    {
        private readonly RestaurantDeliveryContext _context;
        /// <summary>
        /// Конструктор контроллера, принимающий в качестве параметра контекст БД
        /// </summary>
        /// <param name="context">Контекст БД</param>
        public IngredientStringController(RestaurantDeliveryContext context)
        {
            _context = context;
        }
        /// <summary>
        /// Получение строки ингредиента по id из БД
        /// </summary>
        /// <param name="id">id искомой строки ингредиента</param>
        /// <returns>DTO строки ингредиента</returns>
        // GET api/<IngredientStringController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<IngredientStringDTO>> Get(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            IngredientStringDTO ing = new IngredientStringDTO();
            bool isExist = false;
            await foreach (var str in _context.IngredientStrings)
            {
                if (str.IngredientStringId == id)
                {
                    isExist = true;
                    ing = new IngredientStringDTO
                    {
                        IngredientStringId = str.IngredientStringId,
                        IngredientFk = str.IngredientFk,
                        DishFk = str.DishFk,
                    };
                    break;
                }
            }
            if (isExist)
            {
                await foreach (var i in _context.Ingredients)
                {
                    if (i.IngredientId == ing.IngredientFk)
                    {
                        ing.IngredientName = i.IngredientName;
                        break;
                    }
                }
                return Ok(ing);
            }
            return NotFound();
        }
        /// <summary>
        /// Создание новой строки ингредиента в БД
        /// </summary>
        /// <param name="val">DTO новой строки ингредиента</param>
        /// <returns>DTO созданной строки ингредиента</returns>
        // POST api/<IngredientStringController>
        [HttpPost]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult<IngredientStringDTO>> Post([FromBody] IngredientStringDTO val)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            IngredientString ingStr = new IngredientString()
            {
                DishFk = val.DishFk,
                IngredientFk = val.IngredientFk,
            };
            _context.IngredientStrings.Add(ingStr);
            await _context.SaveChangesAsync();
            val.IngredientStringId = ingStr.IngredientStringId;
            return CreatedAtAction("Get", new { id = ingStr.IngredientStringId }, ingStr);
        }
    }
}
