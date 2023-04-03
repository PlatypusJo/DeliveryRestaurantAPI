﻿using Microsoft.AspNetCore.Cors;
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
                    DishCost = Convert.ToString(d.DishCost),
                    DishName = d.DishName,
                    CategoryFk = d.CategoryFk,
                    DishGrammers = Convert.ToString(d.DishGrammers),
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
                        DishCost = Convert.ToString(d.DishCost),
                        DishName = d.DishName,
                        CategoryFk = d.CategoryFk,
                        DishGrammers = Convert.ToString(d.DishGrammers),
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
        public async Task<ActionResult<DishDTO>> Put([FromRoute] int id, [FromBody] DishDTO dishDTO)
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
            dish.DishImage = dish.DishImage;
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
            if (!isExist)
            {
                return NotFound();
            }
            _context.Dishes.Update(dish);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        // DELETE api/<DishController>/5
        [HttpDelete("{id}")]
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
            _context.Dishes.Remove(dish);
            await _context.SaveChangesAsync();
            return Ok("Сущность успешно удалена");
        }
    }
}
