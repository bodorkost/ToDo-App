using Core.Models;
using System.Collections.Generic;

namespace ToDo_App.Models
{
    public class TreeModel
    {
        public TodoItem TodoItem { get; set; }

        public IEnumerable<TreeModel> Children { get; set; }
    }
}
