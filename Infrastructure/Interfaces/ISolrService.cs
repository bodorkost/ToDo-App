using Core.Entities;
using System.Collections.Generic;

namespace Infrastructure.Interfaces
{
    public interface ISolrService
    {
        IEnumerable<TodoItem> Search(string searchText);

        void PopulateData();
    }
}
