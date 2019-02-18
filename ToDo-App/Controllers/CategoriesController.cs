using System;
using Core.Entities;
using Infrastructure.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ToDo_App.Extensions;

namespace ToDo_App.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryService _categoryService;


        public CategoriesController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpPost]
        public IActionResult Create([FromBody] Category category)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.GetErrors());
            }

            _categoryService.Create(category);

            return Ok("Item successfully added to list.");
        }

        [HttpGet]
        public IActionResult Read()
        {
            return Ok(_categoryService.GetAll());
        }

        [HttpGet("{id}")]
        public IActionResult Read(Guid id)
        {
            var readItem = _categoryService.GetById(id);
            if (readItem == null)
            {
                return BadRequest("Item does not exist in list.");
            }

            return Ok(readItem);
        }

        [HttpPatch("{id}")]
        public IActionResult Update(Guid id, [FromBody] Category category)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.GetErrors());
            }

            try
            {
                var updateItem = _categoryService.Edit(id, category);
                if (updateItem == null)
                {
                    return BadRequest("Item does not exist in list.");
                }
            }
            catch(DbUpdateConcurrencyException)
            {
                return Conflict("Conflict has been found.");
            }
            
            return Ok("Item successfully updated.");
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(Guid id)
        {
            var removeItem = _categoryService.Delete(id);
            if (removeItem == null)
            {
                return BadRequest("Item does not exist in list.");
            }

            return Ok("Item successfully removed from list.");
        }
    }
}
