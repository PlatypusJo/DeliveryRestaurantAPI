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
    public class DishController : ControllerBase
    {
        private readonly RestaurantDeliveryContext _context;
        public DishController(RestaurantDeliveryContext context)
        {
            _context = context;
        }
        // GET: api/<DishController>
        [HttpGet]
        public async Task<ActionResult<List<DishDTO>>> GetDish()
        {
            List<DishDTO> dishDTOs = new List<DishDTO>();
            await foreach(var d in _context.Dishes)
            {
                DishDTO dishDTO = new DishDTO()
                {
                    DishId = d.DishId,
                    DishCost = d.DishCost,
                    DishName = d.DishName,
                    CategoryFk = d.CategoryFk,
                    DishGrammers = d.DishGrammers,
                    DishImage = d.DishImage
                };
                dishDTOs.Add(dishDTO);
            }
            foreach (var d in dishDTOs)
            {
                await foreach (var c in _context.Categories)
                {
                    if (c.CategoryId == d.CategoryFk)
                    {
                        d.CategoryName = c.CategoryName;
                        break;
                    }
                }
            }
            foreach(var d in dishDTOs)
            {
                await foreach (var str in _context.IngredientStrings)
                {
                    if (str.DishFk == d.DishId)
                    {
                        IngredientStringDTO ing = new IngredientStringDTO
                        {
                            IngredientStringId = str.IngredientStringId,
                            IngredientFk = str.IngredientFk,
                            DishFk = str.DishFk,
                        };
                        d.IngredientStringsDTO.Add(ing);
                    }
                }
                foreach (var str in d.IngredientStringsDTO)
                {
                    var i = await _context.Ingredients.FindAsync(str.IngredientFk);
                    if (i == null)
                    {
                        return NotFound();
                    }
                    str.IngredientName = i.IngredientName;
                }
            }
            return dishDTOs;
        }

        // GET api/<DishController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<DishDTO>> GetDish(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            DishDTO dishDTO = new DishDTO();
            bool isExist = false;
            await foreach (var d in _context.Dishes)
            {
                if (d.DishId == id)
                {
                    isExist = true;
                    dishDTO = new DishDTO()
                    {
                        DishId = d.DishId,
                        DishCost = d.DishCost,
                        DishName = d.DishName,
                        CategoryFk = d.CategoryFk,
                        DishGrammers = d.DishGrammers,
                        DishImage = d.DishImage
                    };
                    break;
                    
                }
            }
            if (isExist)
            {
                await foreach (var c in _context.Categories)
                {
                    if (c.CategoryId == dishDTO.CategoryFk)
                    {
                        dishDTO.CategoryName = c.CategoryName;
                        break;
                    }
                }
                return Ok(dishDTO);
            }
            return NotFound();
        }

        // POST api/<DishController>
        [HttpPost]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult<DishDTO>> Post([FromBody] DishDTO dish)
        {
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                Dish newDish = new Dish()
                {
                    DishId = dish.DishId,
                    DishCost = Convert.ToInt32(dish.DishCost),
                    DishName = dish.DishName,
                    CategoryFk = dish.CategoryFk,
                    DishGrammers = Convert.ToInt32(dish.DishGrammers),
                    DishImage = dish.DishImage
                };
                bool isExist = false;
                await foreach (var category in _context.Categories)
                {
                    if (category.CategoryId == newDish.CategoryFk)
                    {
                        newDish.CategoryFkNavigation = category;
                        dish.CategoryName = category.CategoryName;
                        category.Dishes.Add(newDish);
                        isExist = true;
                        break;
                    }
                }
                if (!isExist)
                {
                    return BadRequest(ModelState);
                }
                _context.Dishes.Add(newDish);
                await _context.SaveChangesAsync();
                dish.DishId = newDish.DishId;
                return CreatedAtAction("GetDish", new { id = dish.DishId }, dish);
            }
        }

        // PUT api/<DishController>/5
        [HttpPut("{id}")]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult<List<DishDTO>>> Put([FromRoute] int id, [FromBody] DishDTO dishDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var dish = await _context.Dishes.FindAsync(id);
            if (dish == null)
            {
                return NotFound();
            }
            bool isExist = false;
            dish.DishCost = Convert.ToInt32(dishDTO.DishCost);
            dish.DishName = dishDTO.DishName;
            dish.CategoryFk = dishDTO.CategoryFk;
            dish.DishGrammers = Convert.ToInt32(dishDTO.DishGrammers);
            dish.DishImage = dishDTO.DishImage;
            await foreach (var category in _context.Categories)
            {
                if (category.CategoryId == dish.CategoryFk)
                {
                    dish.CategoryFkNavigation = category;
                    dishDTO.CategoryName = category.CategoryName;
                    isExist = true;
                    break;
                }
            }
            await foreach(var strIng in _context.IngredientStrings)
            {
                //if (strIng.DishFk == dish.DishId)
                //{
                //    dish.IngredientStrings.Add(strIng);
                //}
            }
            if (!isExist)
            {
                return NotFound();
            }
            List<IngredientString> deleting = new List<IngredientString>();
            foreach(var str in dish.IngredientStrings)
            {
                bool isExisting = false;
                foreach(var newStr in dishDTO.IngredientStringsDTO)
                {
                    if (str.IngredientStringId == newStr.IngredientStringId)
                    {
                        isExisting = true;
                        str.IngredientFk = newStr.IngredientFk;
                        str.DishFk = newStr.DishFk;
                        _context.IngredientStrings.Update(str);
                        await _context.SaveChangesAsync();
                    }
                }
                if (!isExisting)
                {
                    //dish.IngredientStrings.Remove(str);
                    //_context.IngredientStrings.Remove(str);
                    //await _context.SaveChangesAsync();
                    deleting.Add(str);
                }
            }
            foreach(var str in deleting)
            {
                _context.IngredientStrings.Remove(str);
                await _context.SaveChangesAsync();
            }
            _context.Dishes.Update(dish);
            await _context.SaveChangesAsync();
            return await GetDish();
        }

        // DELETE api/<DishController>/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var dish = await _context.Dishes.FindAsync(id);
            if (dish == null)
            {
                return NotFound();
            }
            await foreach (var inStr in _context.IngredientStrings)
            {
                if (dish.DishId == inStr.DishFk)
                {
                    _context.IngredientStrings.Remove(inStr);
                }
            }
            _context.Dishes.Remove(dish);
            await _context.SaveChangesAsync();
            return Ok("Сущность успешно удалена");
        }
    }
}
