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
            PlayerAdapter pa = new PlayerAdapter();
            TeamAdapter ta = new TeamAdapter();

            Player p = pa.GetPlayer(PlayerID);

            StandardUI gui = new StandardUI();
            gui.BarText = p.Name + "  (" + ta.GetTeamByPlayer(PlayerID).Name + ")";
            gui.SetupScreen();

            string[] DisplayAttributes = new string[] {"Position", "PreferredSide", "DateOfBirth", "", "Agility", "Attitude", "Speed" ,"Stamina", "", "Wages", "Value"};

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
