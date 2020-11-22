using System;
using System.Collections.Generic;
using System.Linq;
using GoalLine.ConsoleApp.UI;
using GoalLine.Data;
using GoalLine.Structures;
using GoalLine.Processes;
using System.Runtime.CompilerServices;

namespace GoalLine.ConsoleApp
{
    public static class GoalLine
    {
        private static bool LoadFromSaveGame = false;

        static void Main(string[] args)
        {
            Console.WindowHeight = 55;

            Welcome();
            MainGameLoop(LoadFromSaveGame);
        }

        private static bool LoadFromSave()
        {
            bool retVal = false;
            GameIO io = new GameIO();

            List<SaveGameInfo> Saves = io.ListSaveGames();

            StandardUI gui = new StandardUI();
            gui.BarText = "Load a Save Game";
            gui.SetupScreen();

            Menu mnu = new Menu();
            mnu.AddColumn(new MenuColumn("ID", ConsoleColor.Yellow, 5));
            mnu.AddColumn(new MenuColumn("Save Game Name", ConsoleColor.White, 50));
            mnu.AddColumn(new MenuColumn("Save Date", ConsoleColor.White, 20));

            for (int s = 0; s < Saves.Count; s++)
            {
                mnu.AddItem(new MenuItem(Saves[s].Name, new string[] { s.ToString(), Saves[s].Name, Saves[s].SaveDate.ToString("dd/MM/yyyy hh:mm") }));
            }

            MenuReturnData menuRet = mnu.RunMenu();
            if (menuRet.Keypress == MenuKeypressConstants.ESC)
            {
                return false;
            }

            io.SaveGameName = menuRet.ItemID;
            io.LoadGame();

            retVal = true;
            return retVal;
        }

        private static void Welcome()
        {
            string FirstName = "";
            string LastName = "";
            int TeamID = -1;

            Progress("");
            Console.Clear();

            StandardUI gui = new StandardUI();
            gui.BarText = "Welcome to GoalLine";
            gui.SetupScreen();

            Console.Write("Load a save game? ");
            string Load = Console.ReadLine();
            if(Load != "")
            {
                if(Load.ToUpper().StartsWith("Y"))
                {
                    if(LoadFromSave())
                    {
                        LoadFromSaveGame = true;
                        return;
                    }
                }
            }

            // Who are ya?
            gui.BarText = "Start a New Game";
            gui.SetupScreen();

            Console.Write("Enter your first name: ");
            FirstName = Console.ReadLine();

            Console.Write("Enter your surname: ");
            LastName = Console.ReadLine();

            Manager you = new Manager();
            you.FirstName = FirstName;
            you.LastName = LastName;
            you.Human = true;
            you.DateOfBirth = DateTime.Now.AddYears(-40); // TODO: Get real DOB
            you.Reputation = 50;

            ManagerAdapter ma = new ManagerAdapter();
            ma.AddManager(you);// TODO: "Next Manager ID" function, must edit the assign manager below

            Console.WriteLine("");
            Progress("Generating Game World...");

            Initialiser init = new Initialiser();

            init.CreateWorld();

            Progress("");

            // Choose Team

            int LeagueID = 0;
            LeagueAdapter la = new LeagueAdapter();
            int MaxLeagueID = la.GetLeagues().Count - 1;

            while(TeamID == -1)
            {
                gui.BarText = "Select a Team";
                gui.SetupScreen();

                Menu mnu = new Menu();
                mnu.AddColumn(new MenuColumn("ID", ConsoleColor.Yellow, 5));
                mnu.AddColumn(new MenuColumn("Team Name", ConsoleColor.White, 35));
                mnu.AddColumn(new MenuColumn("League", ConsoleColor.White, 20));
                mnu.AddExtraKeypress("P", "Preview Team");
                mnu.AddExtraKeypress("+", "Next League");

                TeamAdapter ta = new TeamAdapter();
                foreach (Team t in ta.GetTeamsByLeague(LeagueID))
                {
                    mnu.AddItem(new MenuItem(t.UniqueID.ToString(), new string[] { t.UniqueID.ToString(), t.Name, la.GetLeague(LeagueID).Name }));
                }

                MenuReturnData menuRet = mnu.RunMenu();

                if (menuRet.Keypress == MenuKeypressConstants.ESC)
                {
                    Console.WriteLine("ESC");

                } else if (menuRet.Keypress == MenuKeypressConstants.ENTER)
                {
                    TeamID = Int32.Parse(menuRet.ItemID);

                    ManagerAdapter a = new ManagerAdapter();
                    a.AssignToTeam(0, TeamID);
                } else
                {
                    if(menuRet.Keypress == "P")
                    {
                        TeamDisplay td = new TeamDisplay();
                        td.TeamID = Int32.Parse(menuRet.ItemID);
                        td.Display();
                    } else if(menuRet.Keypress == "+")
                    {
                        LeagueID++;
                        if(LeagueID > MaxLeagueID)
                        {
                            LeagueID = 0;
                        }
                    }
                }
            }


        }

        private static void MainGameLoop(bool SaveGameJustLoaded)
        {
            bool running = true;

            while(running)
            {
                ProcessManager.RunStartOfDay(SaveGameJustLoaded);
                SaveGameJustLoaded = false; // Need to reset this as we only need to skip this the first time

                ManagerAdapter ma = new ManagerAdapter();
                List<Manager> HumanManagers = ma.GetHumanManagers();

                WorldAdapter wa = new WorldAdapter();

                foreach (Manager mgr in HumanManagers)
                {
                    if(mgr.Human)
                    {
                        wa.CurrentManagerID = mgr.UniqueID;
                        MenuDisplay m = new MenuDisplay();
                        m.Display();
                    }
                }
                
                ProcessManager.RunEndOfDay();
            }
        }

        public static void FixtureTest()
        {
            FixtureAdapter fa = new FixtureAdapter();
            List<DateTime> dates = fa.GetDistinctDatesForLeagueMatches(0);

            foreach(DateTime d in dates)
            {
                List<Fixture> FixList = fa.GetFixtures(0, d);
                Console.Clear();

                Console.WriteLine(d);
                Console.WriteLine("");

                TeamAdapter ta = new TeamAdapter();


                foreach(Fixture f in FixList)
                {
                    Console.Write(ta.GetTeam(f.TeamIDs[0]).Name);
                    Console.SetCursorPosition(30, Console.CursorTop);
                    Console.Write(ta.GetTeam(f.TeamIDs[1]).Name);

                    Console.SetCursorPosition(70, Console.CursorTop);
                    Console.Write(f.TeamIDs[0]);

                    Console.SetCursorPosition(74, Console.CursorTop);
                    Console.Write(f.TeamIDs[1]);

                    Console.WriteLine("");
                }

                Console.ReadLine();

            }


        }

        public static void UserError(string Message)
        {
            Console.WriteLine(String.Format("{0} - Press [Enter]", Message));
            Console.ReadLine();
        }

        public static void Progress(string Message)
        {
            if(Message == "")
            {
                Console.Title = "GoalLine - By Paul Turner";
            } else
            {
                Console.Title = Message;
                Console.WriteLine(String.Format("{0}...", Message));
            }
        }
    }
}
