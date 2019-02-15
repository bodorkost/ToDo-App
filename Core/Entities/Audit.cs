using System;

namespace Core.Entities
{
    public class Audit
    {
        public Guid Id { get; set; }

        public string Url { get; set; }

        public string Header { get; set; }

        public string Body { get; set; }

        public DateTime Created { get; set; }
    }
}
