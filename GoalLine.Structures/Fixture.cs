using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoalLine.Structures
{
    public class Fixture
    {
        public int UniqueID { get; set; }
        public int LeagueID { get; set; }
        public DateTime Date { get; set; }
        public int[] TeamIDs { get; set; }  = new int[2]; // [0] = Home, [1] = Away
        public bool Played { get; set; }
        public int[] Score { get; set; } = new int[2]; // [0] = Home Score, [1] = Away Score
    }
}
