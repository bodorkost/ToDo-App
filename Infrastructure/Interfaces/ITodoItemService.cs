using Core.Entities;
using System;
using System.Collections.Generic;

namespace Infrastructure.Interfaces
{
    public interface ITodoItemService
    {
        TodoItem Create(TodoItem item);

        TodoItem GetById(Guid id);

        IEnumerable<TodoItem> GetAll();

        TodoItem Edit(Guid id, TodoItem item);

        TodoItem Delete(Guid id);
    }
}
