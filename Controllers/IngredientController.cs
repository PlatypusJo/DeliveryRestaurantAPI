using Microsoft.AspNetCore.Mvc;
using BackAPI.DTO;
using Microsoft.AspNetCore.Cors;
using BackAPI.Models1;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BackAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IngredientController : ControllerBase
    {
        private readonly RestaurantDeliveryContext _context;
        public IngredientController(RestaurantDeliveryContext context)
        {
            _context = context;
        }
        // GET: api/<IngredientController>
        [HttpGet]
        public async Task<ActionResult<List<Ingredient>>> GetIngredient()
        {
            return await _context.Ingredients.ToListAsync();
        }

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
