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
            you.UniqueID = 0; // TODO: "Next Manager ID" function, must edit the assign manager below

            World.Managers.Add(you);

            Console.WriteLine("");
            Progress("Generating Game World...");

            Initialiser init = new Initialiser();

            init.CreateWorld();

            Progress("");

            // Choose Team
            while(TeamID == -1)
            {
                StandardUI gui = new StandardUI();
                gui.BarText = "Select a Team";
                gui.SetupScreen();

                Menu mnu = new Menu();
                mnu.AddColumn(new MenuColumn("ID", ConsoleColor.Yellow, 5));
                mnu.AddColumn(new MenuColumn("Team Name", ConsoleColor.White, 35));
                mnu.AddExtraKeypress("P", "Preview Team");
            
                foreach (Team t in World.Teams)
                {
                    mnu.AddItem(new MenuItem(t.UniqueID.ToString(), new string[] { t.UniqueID.ToString(), t.Name }));
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

                foreach(Manager mgr in World.Managers)
                {
                    if(mgr.Human)
                    {
                        World.CurrentManagerID = mgr.UniqueID;
                        MenuDisplay m = new MenuDisplay();
                        m.Display();
                    }
                }
                
                ProcessManager.RunEndOfDay();
            }
        }

        private static void FixtureTest()
        {
            LeagueAdapter la = new LeagueAdapter();
            List<DateTime> dates = la.GetDistinctDatesForLeagueMatches(0);

            foreach(DateTime d in dates)
            {
                List<Fixture> FixList = la.GetFixturesForLeagueForDate(0, d);
                Console.Clear();

                Console.WriteLine(d);
                Console.WriteLine("");

                foreach(Fixture f in FixList)
                {
                    Console.Write(World.Teams[f.TeamIDs[0]].Name);
                    Console.SetCursorPosition(30, Console.CursorTop);
                    Console.Write(World.Teams[f.TeamIDs[1]].Name);

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
