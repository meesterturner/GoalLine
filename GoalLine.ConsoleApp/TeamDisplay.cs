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
                
                WorldAdapter wa = new WorldAdapter();
                int CurrentManager = wa.CurrentManagerID;

                TeamAdapter ta = new TeamAdapter();
                Team ThisManagersTeam = ta.GetTeamByManager(CurrentManager);
                bool TeamIsCurrentHumanManager = false;
                if(ThisManagersTeam != null && TeamID == ThisManagersTeam.UniqueID)
                {
                    TeamIsCurrentHumanManager = true;
                }

                ManagerAdapter ma = new ManagerAdapter();
                gui.BarText = ta.GetTeam(TeamID).Name + " - " + ma.GetManager(ta.GetTeam(TeamID).ManagerID).Name;
                if(RedrawAll)
                {
                    gui.SetupScreen();
                }
                
                Menu mnu = new Menu();
                mnu.AddColumn(new MenuColumn("Name", ConsoleColor.White, 40));
                mnu.AddColumn(new MenuColumn("Position", ConsoleColor.Yellow, 10));
                mnu.AddColumn(new MenuColumn(TeamIsCurrentHumanManager ? "Sel" : "Last", ConsoleColor.Cyan, 7));
                mnu.AddColumn(new MenuColumn("Stars", ConsoleColor.Cyan, 10));
                Dictionary<int, TeamPlayer> tp;

                if (TeamIsCurrentHumanManager)
                {
                    mnu.AddExtraKeypress(" ", "Change Start/Sub Selection");
                    tp = ta.GetTeamPlayerSelections(TeamID); // Show current pick (because we can change it)
                } else
                {
                    tp = ta.GetTeamPlayerSelections(TeamID, false); // Show last known pick
                }

                List<Player> Players = ta.GetPlayersInTeam(TeamID);
                

                foreach (Player p in Players)
                {
                    string selText;

                    if(tp.ContainsKey(p.UniqueID))
                    {
                        selText = SelectionText(tp[p.UniqueID].Selected);
                    } else
                    {
                        selText = SelectionText(PlayerSelectionStatus.None);
                    }
                    mnu.AddItem(new MenuItem(p.UniqueID.ToString(), new string[] { p.Name, p.PositionAndSideTextCode, selText, new string(Convert.ToChar("*"), p.Stars) }));
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
