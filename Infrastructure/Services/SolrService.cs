using Core.Entities;
using Infrastructure.Interfaces;
using SolrNet;
using System.Linq;
using System.Collections.Generic;
using Core.Settings;
using Microsoft.Extensions.Options;

namespace Infrastructure.Services
{
    public class SolrService : ISolrService
    {
        private readonly ISolrOperations<TodoItem> _solr;
        private readonly ITodoItemService _todoItemService;
        private readonly IOptions<TodoSettings> _config;

        public SolrService(ISolrOperations<TodoItem> solr, ITodoItemService todoItemService, IOptions<TodoSettings> config)
        {
            _solr = solr;
            _todoItemService = todoItemService;
            _config = config;
        }

        public void PopulateData()
        {
            _solr.Delete(SolrQuery.All);
            _solr.AddRange(_todoItemService.GetAll());
            _solr.Commit();
        }

        public IEnumerable<TodoItem> Search(string searchText)
        {
            var condition = string.IsNullOrEmpty(searchText) ? "*:*" : string.Join($":{searchText} OR ", _config.Value.SolrFilterColumns) + $":{searchText}";
            return _solr.Query(new SolrQuery(condition)).ToList();
        }
    }
}
