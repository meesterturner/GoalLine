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
            string lastSelectedPlayer = "";
            bool RedrawAll = true;

            while(true)
            {
                StandardUI gui = new StandardUI();
                
                TeamAdapter ta = new TeamAdapter();
                gui.BarText = ta.GetTeam(TeamID).Name;
                if(RedrawAll)
                {
                    gui.SetupScreen();
                }
                

                Menu mnu = new Menu();
                mnu.AddColumn(new MenuColumn("Name", ConsoleColor.White, 40));
                mnu.AddColumn(new MenuColumn("Position", ConsoleColor.Yellow, 10));
                mnu.AddColumn(new MenuColumn("Sel", ConsoleColor.Cyan, 7));

                WorldAdapter wa = new WorldAdapter();
                int CurrentManager = wa.CurrentManagerID;

                if(TeamID == ta.GetTeamByManager(CurrentManager).UniqueID)
                {
                    mnu.AddExtraKeypress(" ", "Change Start/Sub Selection");
                }

                List<Player> Players = ta.GetPlayersInTeam(TeamID);
                Dictionary<int, TeamPlayer> tp = ta.GetTeamPlayerSelections(TeamID);

                foreach (Player p in Players)
                {
                    mnu.AddItem(new MenuItem(p.UniqueID.ToString(), new string[] { p.Name, p.PositionAndSideTextCode, SelectionText(tp[p.UniqueID].Selected) }));
                }

                MenuReturnData menuRet = mnu.RunMenu(lastSelectedPlayer);
                RedrawAll = true;
                lastSelectedPlayer = menuRet.ItemID;
                if (menuRet.Keypress == MenuKeypressConstants.ENTER)
                {
                    PlayerDisplay pd = new PlayerDisplay();
                    pd.PlayerID = Int32.Parse(menuRet.ItemID);
                    pd.Display();
                }
                else if(menuRet.Keypress == " ")
                {
                    PlayerSelectionStatus s = ta.CycleTeamPlayerSelection(TeamID, Int32.Parse(menuRet.ItemID));
                    mnu.UpdateItemText(menuRet.ItemID, 2, SelectionText(s));
                    RedrawAll = false;
                } else 
                {
                    break;
                }
            }
            
        }

        private string SelectionText(PlayerSelectionStatus Status)
        {
            switch (Status)
            {
                case PlayerSelectionStatus.Starting:
                    return "PLAY";

                case PlayerSelectionStatus.Sub:
                    return "SUB";

                case PlayerSelectionStatus.None:
                    return "";

                default:
                    throw new NotImplementedException();
            }
        }
    }
}
