using Microsoft.AspNetCore.Cors;
using BackAPI.Models1;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BackAPI.DTO;
using System.Text.RegularExpressions;
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
        public async Task<ActionResult<IEnumerable<Category>>> GetCategory()
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
        public async Task<ActionResult<Category>> Post([FromBody] Category category)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            _context.Categories.Add(category);
            await _context.SaveChangesAsync();
            return CreatedAtAction("GetUser", new { id = category.CategoryId }, category);
        }

        // PUT api/<CategoryController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<CategoryController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
