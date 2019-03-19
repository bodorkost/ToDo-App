using System.Collections.Generic;

namespace Core.Settings
{
    public class TodoSettings
    {
        public int RecentHours { get; set; }
        public string RepoOwner { get; set; }
        public string RepoName { get; set; }
        public IEnumerable<string> SolrFilterColumns { get; set; }
    }
}
