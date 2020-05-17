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
    class MenuDisplay
    {
        public void Display()
        {
            StandardUI gui = new StandardUI();
            gui.BarText = World.Managers[World.CurrentManagerID].Name + " - " + 
                          World.Teams[World.Managers[World.CurrentManagerID].CurrentTeam].Name;
            gui.SetupScreen();

            Menu mnu = new Menu();
            mnu.AddColumn(new MenuColumn("Select an Option", ConsoleColor.White, 40));

            mnu.AddItem(new MenuItem("NOWT", new string[] { "Do Nothing Today" }));

            mnu.AddItem(new MenuItem("TEAM", new string[] { "View My Team" }));
            mnu.AddItem(new MenuItem("FIX", new string[] { "View League Fixtures" }));

            mnu.AddItem(new MenuItem("QUIT", new string[] { "Quit Game" }));

            MenuReturnData menuRet = mnu.RunMenu();
            switch(menuRet.ItemID)
            {
                case "NOWT":
                    break;

                case "TEAM":
                    TeamDisplay td = new TeamDisplay();
                    td.TeamID = World.Managers[World.CurrentManagerID].CurrentTeam;
                    td.Display();
                    break;

                case "FIX":
                    throw new NotImplementedException();
                    break;

                case "QUIT":
                    Environment.Exit(0);
                    break;
            }
        }
        
    }
}
