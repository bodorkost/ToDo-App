using System;
using System.Collections.Generic;
using System.Linq;
using Core.Models;
using Core.Types;
using Microsoft.AspNetCore.Mvc;
using Infrastructure.Data;
using Core.Interfaces;

namespace ToDo_App.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TodosController : ControllerBase
    {
        private readonly ITodoItemService _todoItemService;

        public TodosController(ITodoItemService todoItemService)
        {
            _todoItemService = todoItemService;
        }

        public IActionResult Status()
        {
            return Ok(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
        }

        [HttpPost]
        public IActionResult Create(TodoItem item)
        {
            var validationResult = ValidateItem(item);
            if (validationResult is BadRequestObjectResult)
            {
                return BadRequest((validationResult as BadRequestObjectResult).Value);
            }

            _todoItemService.Create(item);

            return Ok("Item successfully added to list.");
        }

        [HttpPatch("{id}")]
        public IActionResult Update(Guid id, TodoItem item)
        {
            var validationResult = ValidateItem(item);
            if (validationResult is BadRequestObjectResult)
            {
                return BadRequest((validationResult as BadRequestObjectResult).Value);
            }

            var updateItem = _todoItemService.Edit(id, item);
            if (updateItem == null)
            {
                return BadRequest("Item does not exist in list.");
            }

            return Ok("Item successfully updated.");
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(Guid id)
        {
            var removeItem = _todoItemService.Delete(id);
            if (removeItem == null)
            {
                return BadRequest("Item does not exist in list.");
            }

            return Ok("Item successfully removed from list.");
        }

        [HttpGet("{id}")]
        public IActionResult Read(Guid id)
        {
            var readItem = _todoItemService.GetById(id);
            if (readItem == null)
            {
                return BadRequest("Item does not exist in list.");
            }

            return Ok(readItem);
        }

        [HttpGet]
        public IActionResult Read()
        {
            return Ok(_todoItemService.GetAll());
        }

        private IActionResult ValidateItem(TodoItem item)
        {
            if (string.IsNullOrEmpty(item.Name))
            {
                return BadRequest("Name cannot be empty.");
            }
            if (item.Priority == Priority.NONE)
            {
                return BadRequest("Priority cannot be empty.");
            }
            if (item.Deadline.Date < DateTime.Now.Date)
            {
                return BadRequest("Deadline must be later than actual date.");
            }

            return Ok();
        }
    }
}
