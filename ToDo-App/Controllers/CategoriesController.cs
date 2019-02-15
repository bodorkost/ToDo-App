using System;
using Core.Entities;
using Infrastructure.Interfaces;
using Microsoft.AspNetCore.Mvc;
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
            return Ok();
        }

        [HttpPatch("{id}")]
        public IActionResult Update(Guid id, [FromBody] Category category)
        {
            return Ok();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(Guid id)
        {
            return Ok();
        }
    }
}
