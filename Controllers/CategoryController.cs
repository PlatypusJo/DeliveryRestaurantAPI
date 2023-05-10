using Microsoft.AspNetCore.Cors;
using BackAPI.Models1;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BackAPI.DTO;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Authorization;
using System.Data;
// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BackAPI.Controllers
{
    [Route("api/[controller]")]
    [EnableCors]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly RestaurantDeliveryContext _context;
        public CategoryController(RestaurantDeliveryContext context)
        {
            _context = context;
        }

        // GET: api/<CategoryController>
        [HttpGet]
        public async Task<ActionResult<List<Category>>> GetCategory()
        {
            return await _context.Categories.ToListAsync();
        }

        // GET api/<CategoryController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Category>> GetCategory(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category == null)
            {
                return NotFound();
            }
            return Ok(category);
        }

        // POST api/<CategoryController>
        [HttpPost]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult<Category>> Post([FromBody] Category category)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            Category category1 = new Category()
            {
                CategoryName = category.CategoryName,
            };
            _context.Categories.Add(category);
            await _context.SaveChangesAsync();
            category.CategoryId = category1.CategoryId;
            return CreatedAtAction("GetUser", new { id = category.CategoryId }, category);
        }

        // PUT api/<CategoryController>/5
        [HttpPut("{id}")]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult<List<Category>>> Put([FromRoute] int id, [FromBody] Category newCategory)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var category = await _context.Categories.FindAsync(id);
            if (category == null)
            {
                return NotFound();
            }
            category.CategoryName = newCategory.CategoryName;
            _context.Categories.Update(category);
            await _context.SaveChangesAsync();
            return await GetCategory();
        }

        // DELETE api/<CategoryController>/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var category = await _context.Categories.FindAsync(id);
            if (category == null)
            {
                return NotFound();
            }
            List<Dish> dishes = new List<Dish>();
            await foreach (var d in _context.Dishes)
            {
                if (category.CategoryId == d.CategoryFk)
                {
                    dishes.Add(d);
                }
            }
            foreach(var dish in dishes)
            {
                await foreach(var ingStr in _context.IngredientStrings)
                {
                    if (dish.DishId == ingStr.DishFk)
                    {
                        _context.IngredientStrings.Remove(ingStr);
                    }
                }
                _context.Dishes.Remove(dish);
            }
            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();
            return Ok("Сущность успешно удалена");
        }
    }
}
