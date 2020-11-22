using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GoalLine.ConsoleApp.UI;
using GoalLine.Structures;
using GoalLine.Data;
using System.IO;

namespace GoalLine.ConsoleApp
{
    class MenuDisplay
    {
        public void Display()
        {
            bool EndOfDay = false;
            while (!EndOfDay)
            {
                StandardUI gui = new StandardUI();

                WorldAdapter wa = new WorldAdapter();

                TeamAdapter ta = new TeamAdapter();
                Team CurrentTeam = ta.GetTeamByManager(wa.CurrentManagerID);

                ManagerAdapter ma = new ManagerAdapter();
                Manager CurrentManager = ma.GetManager(wa.CurrentManagerID);

                FixtureAdapter fa = new FixtureAdapter();

                gui.BarText = CurrentManager.Name + " - " +
                              CurrentTeam.Name;
                gui.SetupScreen();

                Menu mnu = new Menu();
                mnu.AddColumn(new MenuColumn("Select an Option", ConsoleColor.White, 40));

                if (fa.IsTodayAMatchDay())
                {
                    mnu.AddItem(new MenuItem("PLAY", new string[] { "******** Play Matches ********" }));
                }
                else
                {
                    mnu.AddItem(new MenuItem("NOWT", new string[] { "Do Nothing Today" }));
                }

                mnu.AddItem(new MenuItem("TEAM", new string[] { "View My Team" }));
                mnu.AddItem(new MenuItem("FIX", new string[] { "View League Fixtures" }));
                mnu.AddItem(new MenuItem("TAB", new string[] { "View League Table" }));

                mnu.AddItem(new MenuItem("SAVE", new string[] { "Save Game" }));
                mnu.AddItem(new MenuItem("QUIT", new string[] { "Quit Game" }));

                MenuReturnData menuRet = mnu.RunMenu();
                switch (menuRet.ItemID)
                {
                    case "NOWT":
                        EndOfDay = true;
                        break;

                    case "TEAM":
                        TeamDisplay td = new TeamDisplay();
                        td.TeamID = CurrentManager.CurrentTeam;
                        td.Display();
                        break;

                    case "FIX":
                        FixtureDisplay fd = new FixtureDisplay();
                        fd.Display(CurrentTeam);
                        break;

                    case "PLAY":
                        Matchday md = new Matchday();
                        md.Run();
                        EndOfDay = true;
                        break;

                    case "TAB":
                        LeagueTableDisplay ltd = new LeagueTableDisplay();
                        ltd.Display(CurrentTeam.LeagueID);
                        break;

                    case "SAVE":
                        GameIO i = new GameIO();

                        if(wa.SaveGameName == "" || wa.SaveGameName == null)
                        {
                            gui.BarText = "Save Game";
                            gui.SetupScreen();

                            Console.Write("Enter name for this save game: ");
                            string Name = Console.ReadLine().Trim();

                            if(Name == "")
                            {
                                Name = "Unnamed Save " + DateTime.Now.ToString("yyyy-MM-dd-hhmmss");
                            }

                            wa.SaveGameName = Name.Replace(Path.DirectorySeparatorChar.ToString(), "")
                                                  .Replace(Path.VolumeSeparatorChar.ToString(), "")
                                                  .Replace(Path.AltDirectorySeparatorChar.ToString(), "")
                                                  .Trim();
                        }

                        i.SaveGameName = wa.SaveGameName;
                        i.SaveGame();

                        break; 

                    case "QUIT":
                        Environment.Exit(0);
                        break;
                }
            }
            
        }
        
    }
}
