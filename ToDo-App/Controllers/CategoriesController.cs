using System;
using Core.Entities;
using Microsoft.AspNetCore.Mvc;

namespace ToDo_App.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        [HttpPost]
        public IActionResult Create([FromBody] Category category)
        {
            return Ok();
        }

        [HttpGet]
        public IActionResult Read()
        {
            return Ok();
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
