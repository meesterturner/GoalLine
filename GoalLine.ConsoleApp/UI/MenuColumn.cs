using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoalLine.ConsoleApp.UI
{
    public class MenuColumn
    {
        public string Title { get; set; }
        public ConsoleColor Colour { get; set; }
        public int Width { get; set; }

        public MenuColumn()
        {
            Title = "";
            Colour = ConsoleColor.Gray;
            Width = 20;
        }

        public MenuColumn(string title, ConsoleColor colour, int width)
        {
            Title = title;
            Colour = colour;
            Width = width;
        }
    }
}
