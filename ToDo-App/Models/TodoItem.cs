using System;

namespace ToDo_App.Models
{
    public enum Category
    {
        NONE,
        BUG,
        TASK,
        EPIC
    }

    public class TodoItem
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Priority { get; set; }
        public string Responsible { get; set; }
        public DateTime Deadline { get; set; }
        public int Status { get; set; }
        public Category Category { get; set; }
        public Guid? ParentId { get; set; }
    }
}
