using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoalLine.Structures
{
    public class League
    {
        public int UniqueID { get; set; }
        public string Name { get; set; }
    }

    public class LeagueTableRecord // If anyone questions this, it's because we might need to add our own custom bits in later
    {
        public int TeamID { get; set; }
        public string Name { get; set; }
        public TeamStats SeasonStatistics { get; set; }
    }
}
