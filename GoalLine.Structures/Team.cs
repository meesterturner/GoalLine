using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoalLine.Structures
{
    public class Team
    {
        public int UniqueID { get; set; }
        public string Name { get; set; }
        public List<int> PlayerIDs { get; set; } = new List<int>();
        public int ManagerID { get; set; }
        public int LeagueID { get; set; }

    }
}
