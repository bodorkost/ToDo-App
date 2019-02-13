using System;
using System.Collections.Generic;
using System.Linq;
using Core.Models;
using Core.Types;
using Microsoft.AspNetCore.Mvc;
using Infrastructure.Data;

namespace ToDo_App.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TodosController : ControllerBase
    {
        TodoContext _context;

        public TodosController(TodoContext context)
        {
            _context = context;
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

            item.Id = Guid.NewGuid();
            _context.TodoItems.Add(item);
            _context.SaveChanges();

            return Ok("Item successfully added to list.");
        }

        [HttpPatch("{id}")]
        public IActionResult Update(Guid id, TodoItem item)
        {
            var updateItem = _context.TodoItems.Find(id);
            if (updateItem == null)
            {
                return BadRequest("Item does not exist in list.");
            }

            var validationResult = ValidateItem(item);
            if (validationResult is BadRequestObjectResult)
            {
                return BadRequest((validationResult as BadRequestObjectResult).Value);
            }

            updateItem.Name = item.Name;
            updateItem.Description = item.Description;
            updateItem.Priority = item.Priority;
            updateItem.Responsible = item.Responsible;
            updateItem.Deadline = item.Deadline;
            updateItem.Status = item.Status;
            updateItem.Category = item.Category;
            updateItem.ParentId = item.ParentId;

            _context.SaveChanges();

            return Ok("Item successfully updated.");
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(Guid id)
        {
            var removeItem = _context.TodoItems.Find(id);
            if (removeItem == null)
            {
                return BadRequest("Item does not exist in list.");
            }

            _context.TodoItems.Remove(removeItem);
            _context.SaveChanges();

            return Ok("Item successfully removed from list.");
        }

        [HttpGet("{id}")]
        public IActionResult Read(Guid id)
        {
            var readItem = _context.TodoItems.Find(id);
            if (readItem == null)
            {
                return BadRequest("Item does not exist in list.");
            }

            return Ok(readItem);
        }

        [HttpGet]
        public IActionResult Read()
        {
            return Ok(_context.TodoItems.AsEnumerable());
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
