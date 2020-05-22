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
        public MatchSegment Segment { get; set; }
        public MatchEventType EventType { get; set; }
        public int SegmentTimeSeconds { get; set; }

        public void MatchFinished(Fixture f)
        {
            PrintFixture(f, false);
        }

        public void MatchStarting(Fixture f)
        {
            PrintFixture(f, true);
        }

        private void PrintFixture(Fixture f, bool Starting)
        {
            TeamAdapter ta = new TeamAdapter();

            Console.ForegroundColor = ConsoleColor.White;
            Console.Write(ta.GetTeam(f.TeamIDs[0]).Name);

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write(" v ");

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
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write(Commentary);

            if(EventType == MatchEventType.None)
            {
                System.Threading.Thread.Sleep(50);
            } else
            {
                System.Threading.Thread.Sleep(500);
            }

            Console.SetCursorPosition(0, Console.CursorTop);
        }

        private string DisplayMatchTime()
        {
            return LeadingZero(SegmentTimeSeconds / 60, 2) + ":" + LeadingZero(SegmentTimeSeconds % 60, 2);
        }

        private string LeadingZero(int NumberToDisplay, int Length)
        {
            string retVal;
            retVal = new string(Convert.ToChar("0"), Length) + NumberToDisplay.ToString().Trim();
            return retVal.Substring(retVal.Length - 2, Length);
        }
    }
}
