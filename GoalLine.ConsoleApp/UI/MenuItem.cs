using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoalLine.ConsoleApp.UI
{
    public class MenuItem
    {
        public string Id { get; set; }
        public string[] DisplayText { get; set; }

        public MenuItem(string id, string[] displayText)
        {
            Id = id;
            DisplayText = displayText;
        }
    }
}
