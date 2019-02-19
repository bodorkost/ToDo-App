using Core.Entities;
using Core.Types;
using Infrastructure.Data;
using System;

namespace Todo_App.Tests
{
    public class DummyDataDBInitializer
    {
        public DummyDataDBInitializer()
        {
        }

        public void Seed(TodoContext dbContext)
        {
            dbContext.Database.EnsureDeleted();
            dbContext.Database.EnsureCreated();

            var now = DateTime.Now;
            var bugId = Guid.NewGuid();
            var epicId = Guid.NewGuid();
            var taskId = Guid.NewGuid();

            dbContext.Categories.AddRange(
                new Category() { Id = bugId, DisplayName = "BUG", Created = now, Modified = now },
                new Category() { Id = epicId, DisplayName = "EPIC", Created = now, Modified = now },
                new Category() { Id = taskId, DisplayName = "TASK", Created = now, Modified = now }
            );

            dbContext.TodoItems.AddRange(
                new TodoItem() { Id = Guid.NewGuid(), Name = "Todo 1", CategoryId = bugId, Priority = Priority.IMPORTANT, Created = now, Modified = now },
                new TodoItem() { Id = Guid.NewGuid(), Name = "Todo 2", CategoryId = bugId, Priority = Priority.IMPORTANT, Created = now, Modified = now },
                new TodoItem() { Id = Guid.NewGuid(), Name = "Todo 3", CategoryId = bugId, Priority = Priority.IMPORTANT, Created = now, Modified = now },
                new TodoItem() { Id = Guid.NewGuid(), Name = "Todo 4", CategoryId = epicId, Priority = Priority.IMPORTANT, Created = now, Modified = now }


            );

            dbContext.SaveChanges();
        }
    }
}
