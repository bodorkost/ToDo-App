using Core.Entities;
using System.Collections.Generic;

namespace Infrastructure.Interfaces
{
    public interface ITodoItemService : IBaseService<TodoItem>
    {
        IEnumerable<TodoItem> GetMyTodosFromSql(string responsible);
    }
}
