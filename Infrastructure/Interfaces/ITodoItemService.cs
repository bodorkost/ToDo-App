using Core.Entities;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Infrastructure.Interfaces
{
    public interface ITodoItemService : IBaseService<TodoItem>
    {
        Task<IEnumerable<TodoItem>> GetMyTodosFromSql(string responsible);

        IEnumerable<TodoItem> SolrSearch(string searchText);

        void SolrRefresh();
    }
}
