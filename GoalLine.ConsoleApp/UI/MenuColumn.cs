using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace GoalLine.ConsoleApp.UI
{
    public enum MenuColumnAlignment
    {
        Left = 0,
        Right = 99
    }

    public class MenuColumn
    {

        public string Title { get; set; }
        public ConsoleColor Colour { get; set; }
        public int Width { get; set; }
        public MenuColumnAlignment Alignment { get; set; }

        public MenuColumn()
        {
            Title = "";
            Colour = ConsoleColor.Gray;
            Width = 20;
            Alignment = MenuColumnAlignment.Left;
        }

        public MenuColumn(string title, ConsoleColor colour, int width)
        {
            Title = title;
            Colour = colour;
            Width = width;
            Alignment = MenuColumnAlignment.Left;
        }

        public MenuColumn(string title, ConsoleColor colour, int width, MenuColumnAlignment alignment)
        {
            Title = title;
            Colour = colour;
            Width = width;
            Alignment = alignment;
        }
    }
}
