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

            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("*** START ***");

            MatchPlayer mp = new MatchPlayer();
            FixtureAdapter fa = new FixtureAdapter();
            mp.Fixture = fa.GetFixture(0);
            mp.Interactive = true;
            mp.MatchCallback = new MatchdayCallback();
            mp.StartMatch();

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("*** END ***");

            Console.ReadLine();
        }
    }

    class MatchdayCallback : IMatchCallback
    {
        public string Commentary { get; set; }
        public MatchSegment Segment { get; set; }
        public MatchEventType EventType { get; set; }
        public int SegmentTimeSeconds { get; set; }

        public void UpdateUI()
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write("[ " + DisplayMatchTime() + " ] - ");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(Commentary);

            if(EventType == MatchEventType.None)
            {
                System.Threading.Thread.Sleep(50);
            } else
            {
                System.Threading.Thread.Sleep(500);
            }
            
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
