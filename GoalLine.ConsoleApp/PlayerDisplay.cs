using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GoalLine.ConsoleApp.UI;
using GoalLine.Structures;
using GoalLine.Data;
using System.Security.Cryptography.X509Certificates;

namespace GoalLine.ConsoleApp
{
    class PlayerDisplay
    {
        public int PlayerID { get; set; }

        public void Display()
        {
            Player p = World.Players[PlayerID];

            StandardUI gui = new StandardUI();
            gui.BarText = p.Name + "  (" + World.Teams[p.CurrentTeam].Name + ")";
            gui.SetupScreen();

            string[] DisplayAttributes = new string[] {"Position", "PreferredSide", "DateOfBirth", "", "Agility", "Attitude", "Speed" ,"Stamina", "Wages"};

            foreach(string a in DisplayAttributes)
            {
                if(a != "")
                {
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.SetCursorPosition(4, Console.CursorTop);
                    Console.Write(a);
                    Console.SetCursorPosition(20, Console.CursorTop);
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine(p.GetType().GetProperty(a).GetValue(p));
                } else
                {
                    Console.WriteLine("");
                }
               
            }

            Console.ReadKey(); // TODO: Better stuff.
        }
    }
}
