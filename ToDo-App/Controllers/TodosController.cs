﻿using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using ToDo_App.Models;

namespace ToDo_App.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TodosController : ControllerBase
    {
        public static IList<TodoItem> _items = new List<TodoItem>
            {
                new TodoItem()
                {
                    Id = Guid.NewGuid(),
                    Name = "First",
                    Description = "This is the first todo in my list.",
                    Priority = 1,
                    Responsible = "People 1",
                    Deadline = new DateTime(2020, 01, 01),
                    Status = 0,
                    Category = Category.BUG,
                    ParentId = null
                },
                new TodoItem()
                {
                    Id = Guid.NewGuid(),
                    Name = "Second",
                    Description = "This is the second todo in my list.",
                    Priority = 2,
                    Responsible = "People 2",
                    Deadline = new DateTime(2019, 05, 16),
                    Status = 0,
                    Category = Category.EPIC,
                    ParentId = null
                },
                new TodoItem()
                {
                    Id = Guid.NewGuid(),
                    Name = "Third",
                    Description = "This is the third todo in my list.",
                    Priority = 3,
                    Responsible = "People 2",
                    Deadline = new DateTime(2019, 12, 12),
                    Status = 0,
                    Category = Category.TASK,
                    ParentId = null
                }
            };

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
            _items.Add(item);

            return Ok("Item successfully added to list.");
        }

        [HttpPatch("{id}")]
        public IActionResult Update(Guid id, TodoItem item)
        {
            var updateItem = _items.FirstOrDefault(i => i.Id == id);
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

            return Ok("Item successfully updated.");
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(Guid id)
        {
            var removeItem = _items.FirstOrDefault(i => i.Id == id);
            if (removeItem == null)
            {
                return BadRequest("Item does not exist in list.");
            }

            _items.Remove(removeItem);

            return Ok("Item successfully removed from list.");
        }

        [HttpGet("{id}")]
        public IActionResult Read(Guid id)
        {
            var readItem = _items.FirstOrDefault(i => i.Id == id);
            if (readItem == null)
            {
                return BadRequest("Item does not exist in list.");
            }

            return Ok(readItem);
        }

        [HttpGet]
        public IActionResult Read()
        {
            if (_items.Count == 0)
            {
                return Ok("List is empty.");
            }

            return Ok(_items);
        }

        private IActionResult ValidateItem(TodoItem item)
        {
            if (string.IsNullOrEmpty(item.Name))
            {
                return BadRequest("Name cannot be empty.");
            }
            if (string.IsNullOrEmpty(item.Description))
            {
                return BadRequest("Description cannot be empty.");
            }
            if (string.IsNullOrEmpty(item.Responsible))
            {
                return BadRequest("Responsible cannot be empty.");
            }
            if (item.Deadline.Date < DateTime.Now.Date)
            {
                return BadRequest("Deadline must be later than actual date.");
            }
            if (item.Category == Category.NONE)
            {
                return BadRequest("Category cannot be empty.");
            }

            return Ok();
        }
    }
}
