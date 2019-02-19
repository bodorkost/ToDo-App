using System;
using System.Linq;
using System.Collections.Generic;
using Core.Types;
using Core.Entities;

namespace Todo_App.Tests
{
    public static class TestHelper
    {
        public static IEnumerable<Category> Categories = new List<Category>()
            {
                new Category() { Id = Guid.NewGuid(), DisplayName = "BUG", Created = DateTime.Now, Modified = DateTime.Now },
                new Category() { Id = Guid.NewGuid(), DisplayName = "EPIC", Created = DateTime.Now, Modified = DateTime.Now },
                new Category() { Id = Guid.NewGuid(), DisplayName = "TASK", Created = DateTime.Now, Modified = DateTime.Now }
            };

        public static IEnumerable<TodoItem> TodoItems = new List<TodoItem>()
            {
                new TodoItem() { Id = Guid.NewGuid(), Name = "Todo 1", CategoryId = Categories.ElementAt(0).Id, Priority = Priority.IMPORTANT, Created = DateTime.Now, Modified = DateTime.Now },
                new TodoItem() { Id = Guid.NewGuid(), Name = "Todo 2", CategoryId = Categories.ElementAt(0).Id, Priority = Priority.CRITICAL, Created = DateTime.Now, Modified = DateTime.Now },
                new TodoItem() { Id = Guid.NewGuid(), Name = "Todo 3", CategoryId = Categories.ElementAt(0).Id, Priority = Priority.LOW, Created = DateTime.Now, Modified = DateTime.Now },
                new TodoItem() { Id = Guid.NewGuid(), Name = "Todo 4", CategoryId = Categories.ElementAt(1).Id, Priority = Priority.NORMAL, Created = DateTime.Now, Modified = DateTime.Now }
            };
    }
}
