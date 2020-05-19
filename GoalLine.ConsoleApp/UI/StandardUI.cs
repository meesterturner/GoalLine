using GoalLine.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoalLine.ConsoleApp.UI
{
    class StandardUI
    {
        public ConsoleColor BarColour { get; set; }
        public ConsoleColor BarTextColour { get; set; }
        public string BarText { get; set; }

        public StandardUI()
        {
            BarColour = ConsoleColor.Blue;
            BarTextColour = ConsoleColor.White;
            BarText = "GoalLine - By Paul Turner";
        }

        public void SetupScreen()
        {
            WorldAdapter wa = new WorldAdapter();

            Console.BackgroundColor = ConsoleColor.Black;
            Console.Clear();

            Console.BackgroundColor = BarColour;
            Console.ForegroundColor = BarTextColour;
            Console.SetCursorPosition(0, 0);
            Console.Write(new string(Convert.ToChar(" "), Console.WindowWidth * 3));
            Console.SetCursorPosition(4, 1);
            Console.Write(BarText);

            Console.SetCursorPosition(Console.WindowWidth - 13, 1);
            Console.Write(wa.CurrentDate.ToString("dd MMM yyyy"));

            Console.BackgroundColor = ConsoleColor.Black;
            Console.SetCursorPosition(0, 4);

        }
    }
}
