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
    class FixtureDisplay
    {
        public void Display(Team T)
        {
            FixtureAdapter fa = new FixtureAdapter();
            Display_Worker(fa.GetFixtures(T.LeagueID, T.UniqueID));
        }

        private void Display_Worker(List<Fixture> Fixtures)
        { 
            while (true)
            {
                StandardUI gui = new StandardUI();

                TeamAdapter ta = new TeamAdapter();
                gui.BarText = "Fixtures"; // TODO: Bit more descriptive
                gui.SetupScreen();

                Menu mnu = new Menu();
                mnu.AddColumn(new MenuColumn("Date", ConsoleColor.White, 12));
                mnu.AddColumn(new MenuColumn("Home", ConsoleColor.White, 40));
                mnu.AddColumn(new MenuColumn(" ", ConsoleColor.Yellow, 10));
                mnu.AddColumn(new MenuColumn("Away", ConsoleColor.White, 40));

                foreach (Fixture f in Fixtures)
                {
                    string Result = "v";
                    if (f.Played)
                    {
                        Result = String.Format("{0} : {1}", f.Score[0], f.Score[1]);
                    }
                    mnu.AddItem(new MenuItem(f.UniqueID.ToString(), new string[] { f.Date.ToString("dd/MM/yyyy"), ta.GetTeam(f.TeamIDs[0]).Name, Result, ta.GetTeam(f.TeamIDs[1]).Name }));
                }

                MenuReturnData menuRet = mnu.RunMenu();
                if (menuRet.Keypress == MenuKeypressConstants.ENTER)
                {
                    FixtureAdapter fa = new FixtureAdapter();
                    Fixture f = fa.GetFixture(Int32.Parse( menuRet.ItemID));

                    int TeamID = f.TeamIDs[0];
                    WorldAdapter wa = new WorldAdapter();
                    if (TeamID ==  new TeamAdapter().GetTeamByManager(wa.CurrentManagerID).UniqueID)
                    {
                        TeamID = f.TeamIDs[1];
                    }

                    TeamDisplay td = new TeamDisplay();
                    td.TeamID = TeamID;
                    td.Display();
                }
                else
                {
                    break;
                }
            }

        }
    }
}
