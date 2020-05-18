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
    class TeamDisplay
    {
        public int TeamID { get; set; }

        public void Display()
        {
            while(true)
            {
                StandardUI gui = new StandardUI();
                
                TeamAdapter ta = new TeamAdapter();
                gui.BarText = ta.GetTeam(TeamID).Name;
                gui.SetupScreen();

                Menu mnu = new Menu();
                mnu.AddColumn(new MenuColumn("Name", ConsoleColor.White, 40));
                mnu.AddColumn(new MenuColumn("Position", ConsoleColor.Yellow, 10));

                
                List<Player> Players = ta.GetPlayersInTeam(TeamID);

                foreach (Player p in Players)
                {
                    mnu.AddItem(new MenuItem(p.UniqueID.ToString(), new string[] { p.Name, p.PositionAndSideTextCode }));
                }

                MenuReturnData menuRet = mnu.RunMenu();
                if (menuRet.Keypress == MenuKeypressConstants.ENTER)
                {
                    PlayerDisplay pd = new PlayerDisplay();
                    pd.PlayerID = Int32.Parse(menuRet.ItemID);
                    pd.Display();
                }
                else
                {
                    break;
                }
            }
            
        }
    }
}
