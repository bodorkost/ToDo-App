using System;
using System.Linq;
using Core.Entities;
using Core.Types;
using Microsoft.AspNetCore.Mvc;
using Infrastructure.Interfaces;
using ToDo_App.Extensions;
using System.Collections.Generic;
using ToDo_App.Models;
using Core.Settings;
using Microsoft.Extensions.Options;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace ToDo_App.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TodosController : ControllerBase
    {
        private readonly ITodoItemService _todoItemService;
        private readonly IRabbitMQService _rabbitMQService;
        private readonly IOptions<TodoSettings> _config;
        private readonly IHostingEnvironment _env;
        private readonly IHttpClientFactory _clientFactory;

        public TodosController(
            ITodoItemService todoItemService,
            IRabbitMQService rabbitMQService,
            IOptions<TodoSettings> config, 
            IHostingEnvironment env,
            IHttpClientFactory clientFactory)
        {
            _todoItemService = todoItemService;
            _rabbitMQService = rabbitMQService;
            _config = config;
            _env = env;
            _clientFactory = clientFactory;
        }

        [Authorize]
        [HttpPost]
        public IActionResult Create(TodoItem item)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.GetErrors());
            }

            try
            {
                _todoItemService.Create(item);
                _rabbitMQService.SendCreate(item);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            return Ok("Item successfully added to list.");
        }

        [Authorize]
        [HttpPatch("{id}")]
        public IActionResult Update(Guid id, TodoItem item)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.GetErrors());
            }

            try
            {
                var updateItem = _todoItemService.Edit(id, item);
                if (updateItem == null)
                {
                    return BadRequest("Item does not exist in list.");
                }
            }
            catch (DbUpdateConcurrencyException)
            {
                return Conflict("Conflict has been found.");
            }
            

            return Ok("Item successfully updated.");
        }

        [Authorize]
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
        public IActionResult ReadByCategory(string category)
        {
            return Ok(_todoItemService.GetAll()
                                      .Include("Category")
                                      .Where(t => t.Category.DisplayName == category));
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
            return Ok(_todoItemService.GetAll().Include("Category").AsEnumerable()
                                      .GroupBy(t => t.Category, (category, todos) => 
                                        new { Category = category.DisplayName, Todos = todos }));
        }

        [HttpGet("Tree")]
        public IActionResult ReadTree()
        {
            //return Ok(FillTreeRecursive(_todoItemService.GetAll()));
            return Ok(FillTree(_todoItemService.GetAll().OrderBy(t => t.Modified)));
        }

        [HttpGet("Thumbnail/{category}")]
        public IActionResult GetCategoryThumbnail(string category)
        {
            var path = Path.Combine(_env.ContentRootPath, "Assets", "Images", "Category", $"{category.ToLower()}.png");

            if (!System.IO.File.Exists(path))
            {
                return NotFound("Category was not found.");
            }
            
            var memory = new MemoryStream();
            using (var stream = new FileStream(path, FileMode.Open))
            {
                stream.CopyTo(memory);
            }
            memory.Position = 0;

            return File(memory, "application /octet-stream", Path.GetFileName(path));
        }

        [HttpGet("MyTodos/{responsible}")]
        public IActionResult GetMyTodos(string responsible)
        {
            return Ok(_todoItemService.GetMyTodosFromSql(responsible));
        }

        [HttpGet("Issues")]
        public async Task<IActionResult> GetGitHubIssues()
        {
            var request = new HttpRequestMessage(HttpMethod.Get, $"repos/{_config.Value.RepoOwner}/{_config.Value.RepoName}/issues");
            var response = await _clientFactory.CreateClient("github").SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                var issues = await response.Content.ReadAsAsync<IEnumerable<GitHubIssueModel>>();
                return Ok(issues);
            }

            return BadRequest($"Request failed with status code {(int)response.StatusCode}");
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
