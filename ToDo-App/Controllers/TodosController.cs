﻿using System;
using System.Linq;
using Core.Models;
using Core.Types;
using Microsoft.AspNetCore.Mvc;
using Infrastructure.Interfaces;
using ToDo_App.Extensions;
using System.Collections.Generic;
using ToDo_App.Models;
using Core;
using Microsoft.Extensions.Options;

namespace ToDo_App.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TodosController : ControllerBase
    {
        private readonly ITodoItemService _todoItemService;
        private readonly IOptions<TodoSettings> _config;

        public TodosController(ITodoItemService todoItemService, IOptions<TodoSettings> config)
        {
            _todoItemService = todoItemService;
            _config = config;
        }

        [HttpPost]
        public IActionResult Create(TodoItem item)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.GetErrors());
            }

            _todoItemService.Create(item);

            return Ok("Item successfully added to list.");
        }

        [HttpPatch("{id}")]
        public IActionResult Update(Guid id, TodoItem item)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.GetErrors());
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

        [HttpGet("ByCategory/{category}")]
        public IActionResult ReadByCategory(Category category)
        {
            return Ok(_todoItemService.GetAll()
                                      .Where(t => t.Category == category));
        }

        [HttpGet("Recent")]
        public IActionResult ReadByRecent()
        {
            return Ok(_todoItemService.GetAll()
                                      .Where(t => t.Status == Status.OPEN 
                                            || (t.Status == Status.CLOSED && t.Modified > DateTime.Now.AddHours(_config.Value.RecentHours) && t.Modified < DateTime.Now)));
        }

        [HttpGet("WithCategory")]
        public IActionResult ReadWithCategory()
        {
            return Ok(_todoItemService.GetAll()
                                      .GroupBy(t => t.Category, (category, todos) => 
                                        new { Category = category.ToString(), Todos = todos }));
        }

        [HttpGet("Tree")]
        public IActionResult ReadTree()
        {
            //return Ok(FillTreeRecursive(_todoItemService.GetAll()));
            return Ok(FillTree(_todoItemService.GetAll().OrderBy(t => t.Modified)));
        }

        private IEnumerable<TreeModel> FillTree(IEnumerable<TodoItem> todos)
        {
            var tree = todos.Select(t => new TreeModel() { TodoItem = t }).ToList();
            var dict = tree.ToDictionary(t => t.TodoItem.Id, t => t);

            foreach (var item in tree)
            {
                if(item.TodoItem.ParentId != null)
                {
                    dict[item.TodoItem.ParentId.Value].Children.Add(item);
                }
            }

            return tree.Where(t => t.TodoItem.ParentId == null);
        }

        private ICollection<TreeModel> FillTreeRecursive(IEnumerable<TodoItem> todos, Guid? parentId = null)
        {
            var tree = new List<TreeModel>();

            foreach (var item in todos.Where(x => x.ParentId.Equals(parentId)))
            {
                tree.Add(new TreeModel
                {
                    TodoItem = item,
                    Children = FillTreeRecursive(todos, item.Id)
                });
            }

            return tree;
        }
    }
}
