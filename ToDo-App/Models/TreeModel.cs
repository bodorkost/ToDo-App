using Core.Entities;
using System.Collections.Generic;

namespace ToDo_App.Models
{
    public class TreeModel
    {
        public TodoItem TodoItem { get; set; }

        public ICollection<TreeModel> Children { get; set; } = new List<TreeModel>();
    }
}
