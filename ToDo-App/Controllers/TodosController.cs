using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using ToDo_App.Models;

namespace ToDo_App.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TodosController : ControllerBase
    {
        public static IList<TodoItem> _items { get; set; }

        public TodosController()
        {
            _items = new List<TodoItem>
            {
                new TodoItem()
                {
                    Id = Guid.NewGuid(),
                    Name = "First",
                    Description = "This is the first todo in my list.",
                    Priority = 1,
                    Responsible = "?",
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
                    Responsible = "?",
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
                    Responsible = "?",
                    Deadline = new DateTime(2019, 12, 12),
                    Status = 0,
                    Category = Category.TASK,
                    ParentId = null
                }
            };
        }

        // GET api/status
        [HttpGet]
        public IActionResult Status()
        {
            return Ok(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
        }
    }
}
