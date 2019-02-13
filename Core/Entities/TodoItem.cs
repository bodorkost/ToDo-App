using System;
using Core.Types;

namespace Core.Models
{
    public class TodoItem
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Priority Priority { get; set; }
        public string Responsible { get; set; }
        public DateTime Deadline { get; set; }
        public int Status { get; set; }
        public Category Category { get; set; }
        public Guid? ParentId { get; set; }
        public DateTime Created { get; set; }
        public string Creator { get; set; }
        public DateTime Modified { get; set; }
        public string Modifier { get; set; }
        public bool IsDeleted { get; set; }
    }
}
