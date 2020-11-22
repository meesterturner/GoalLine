using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GoalLine.ConsoleApp.UI;
using GoalLine.Structures;
using GoalLine.Data;

namespace GoalLine.ConsoleApp
{
    class LeagueTableDisplay
    {
        public void Display(int LeagueID)
        {
            LeagueAdapter la = new LeagueAdapter();
            Display_Worker(la.LeagueTable(LeagueID));
        }

        private void Display_Worker(List<LeagueTableRecord> Records)
        {
            while (true)
            {
                StandardUI gui = new StandardUI();

                TeamAdapter ta = new TeamAdapter();
                gui.BarText = "League Table"; // TODO: Bit more descriptive
                gui.SetupScreen();

                Menu mnu = new Menu();
                mnu.AddColumn(new MenuColumn("Pos", ConsoleColor.White, 4));
                mnu.AddColumn(new MenuColumn("Team", ConsoleColor.White, 40));
                mnu.AddColumn(new MenuColumn("P", ConsoleColor.Yellow, 4, MenuColumnAlignment.Right));
                mnu.AddColumn(new MenuColumn("W", ConsoleColor.Yellow, 4, MenuColumnAlignment.Right));
                mnu.AddColumn(new MenuColumn("D", ConsoleColor.Yellow, 4, MenuColumnAlignment.Right));
                mnu.AddColumn(new MenuColumn("L", ConsoleColor.Yellow, 4, MenuColumnAlignment.Right));
                mnu.AddColumn(new MenuColumn("F", ConsoleColor.Yellow, 4, MenuColumnAlignment.Right));
                mnu.AddColumn(new MenuColumn("A", ConsoleColor.Yellow, 4, MenuColumnAlignment.Right));
                mnu.AddColumn(new MenuColumn("GD", ConsoleColor.Yellow, 4, MenuColumnAlignment.Right));
                mnu.AddColumn(new MenuColumn("Pts", ConsoleColor.Cyan, 6, MenuColumnAlignment.Right));

                int Pos = 1;

                foreach (LeagueTableRecord r in Records)
                {
                    mnu.AddItem(new MenuItem(Pos.ToString(), new string[] { Pos.ToString(), r.Name, r.GamesPlayed.ToString(), r.Won.ToString(), r.Drawn.ToString(), r.Lost.ToString(), 
                                                                            r.GoalsScored.ToString(), r.GoalsConceded.ToString(), r.GoalDifference.ToString(), r.Points.ToString()}));
                    Pos++;
                }

                MenuReturnData menuRet = mnu.RunMenu();
                break;
            }

        }


    }
}
