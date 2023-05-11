using Microsoft.AspNetCore.Mvc;
using BackAPI.DTO;
using BackAPI.Models1;
using Microsoft.EntityFrameworkCore;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BackAPI.Controllers
{

    

    [Route("api/[controller]")]
    [ApiController]
    public class IngredientStringController : ControllerBase
    {
        private readonly RestaurantDeliveryContext _context;
        public IngredientStringController(RestaurantDeliveryContext context)
        {
            _context = context;
        }
        // GET: api/<IngredientStringController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

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

        // POST api/<IngredientStringController>
        [HttpPost]
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

        // PUT api/<IngredientStringController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<IngredientStringController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
