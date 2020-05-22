using System;
using System.Collections.Generic;
using System.Linq;
using GoalLine.ConsoleApp.UI;
using GoalLine.Data;
using GoalLine.Structures;
using GoalLine.Processes;

namespace GoalLine.ConsoleApp
{
    public static class GoalLine
    {
        static void Main(string[] args)
        {
            Console.WindowHeight = 55;

            Welcome();
            MainGameLoop(false); // TODO: If loading a savegame, this needs to be true.
        }

        private static void Welcome()
        {
            string FirstName = "";
            string LastName = "";
            int TeamID = -1;

            Progress("");
            Console.Clear();

            Console.WriteLine("Welcome to GoalLine");
            Console.WriteLine("===================");
            Console.WriteLine("");

            // Who are ya?
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
                StandardUI gui = new StandardUI();
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
