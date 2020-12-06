using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoalLine.Data
{
    public class MatchEventChanceItem
    {
        public MatchEventType evType { get; set; }
        public double Chance { get; set; }

        public MatchEventChanceItem(MatchEventType evType, double Chance)
        {
            this.evType = evType;
            this.Chance = Chance;
        }
    }
}
