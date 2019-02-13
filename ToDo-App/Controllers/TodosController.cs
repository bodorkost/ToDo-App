using System;
using System.Collections.Generic;
using System.Linq;
using Core.Models;
using Core.Types;
using Microsoft.AspNetCore.Mvc;
using Infrastructure.Data;
using Infrastructure.Interfaces;
using Microsoft.AspNetCore.Mvc.ModelBinding;

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
            if (!ModelState.IsValid)
            {
                return BadRequest(
                    ModelState
                        .Where(p => p.Value.ValidationState == ModelValidationState.Invalid)
                        .Select(p => new { key = p.Key, propErrors = p.Value.Errors.Select(e => e.ErrorMessage) })
                );
            }

            _todoItemService.Create(item);

            return Ok("Item successfully added to list.");
        }

        [HttpPatch("{id}")]
        public IActionResult Update(Guid id, TodoItem item)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(
                    ModelState
                        .Where(p => p.Value.ValidationState == ModelValidationState.Invalid)
                        .Select(p => new { key = p.Key, propErrors = p.Value.Errors.Select(e => e.ErrorMessage) })
                );
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
    }
}
