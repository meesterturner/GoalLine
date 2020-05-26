using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using GoalLine.Data;
using GoalLine.Matchday;
using GoalLine.Structures;

namespace GoalLine.ConsoleApp
{
    class Matchday
    {
        public void Run()
        {
            Console.Clear();
            Console.CursorVisible = false;
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("*** START ***");

            MatchDayRunner dr = new MatchDayRunner();
            dr.Run(new MatchdayCallback());

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("*** END ***");

            Console.CursorVisible = true;
            Console.ReadLine();
        }
    }

    class MatchdayCallback : IMatchCallback
    {
        public string Commentary { get; set; }
        public MatchEventType EventType { get; set; }
        public MatchStatus MatchStatus { get; set; }

        public void MatchFinished(Fixture f)
        {
            Console.Write(new string(Convert.ToChar(" "), Console.WindowWidth - 1));
            Console.SetCursorPosition(0, Console.CursorTop);
            PrintFixture(f, false);
        }

        public void MatchStarting(Fixture f)
        {
            //PrintFixture(f, true);
        }

        private void PrintFixture(Fixture f, bool Starting)
        {
            TeamAdapter ta = new TeamAdapter();

            Console.ForegroundColor = ConsoleColor.White;
            Console.Write(ta.GetTeam(f.TeamIDs[0]).Name);

            Console.ForegroundColor = ConsoleColor.Yellow;
            if(Starting)
            {
                Console.Write(" v ");
            } else
            {
                Console.Write(String.Format(" {0} : {1} ", f.Score[0], f.Score[1] ));
            }
            

            Console.ForegroundColor = ConsoleColor.White;
            Console.Write(ta.GetTeam(f.TeamIDs[1]).Name);

            Console.WriteLine(String.Format(" - {0}", Starting ? "START" : "END"));
        }

        public void UpdateUI()
        {
            Console.Write(new string(Convert.ToChar(" "), Console.WindowWidth - 1));
            Console.SetCursorPosition(0, Console.CursorTop);

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write("[ " + DisplayMatchTime() + " ] - ");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write(String.Format(" {0} : {1} ", MatchStatus.Score[0], MatchStatus.Score[1] ));
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write(Commentary);

            Console.Title = String.Format("X: {0}  Possess: {1} [{2} : {3}]  Shots: [{4} : {5}]", 
                                        MatchStatus.BallX, 
                                        MatchStatus.PossessionTeam,
                                        MatchStatus.PossessionPercentage(0),
                                        MatchStatus.PossessionPercentage(1),
                                        MatchStatus.Shots[0],
                                        MatchStatus.Shots[1]);

            if(EventType == MatchEventType.Goal)
            {
                Console.Write("   *** GOAL ***");
                Console.ReadKey(true);
            }
            else if(EventType == MatchEventType.None)
            {
                System.Threading.Thread.Sleep(75);
            } else
            {
                System.Threading.Thread.Sleep(750);
            }

            Console.SetCursorPosition(0, Console.CursorTop);
        }

        private string DisplayMatchTime()
        {
            return LeadingZero(MatchStatus.MatchTimeSeconds / 60, 2) + ":" + LeadingZero(MatchStatus.MatchTimeSeconds % 60, 2);
        }

        private string LeadingZero(int NumberToDisplay, int Length)
        {
            string retVal;
            retVal = new string(Convert.ToChar("0"), Length) + NumberToDisplay.ToString().Trim();
            return retVal.Substring(retVal.Length - 2, Length);
        }
    }
}
