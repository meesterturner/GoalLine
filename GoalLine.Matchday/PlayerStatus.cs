using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GoalLine.Structures;

namespace GoalLine.Matchday
{
    public class PlayerStatus
    {
        public int PlayerID { get; set; }
        public PlayerSelectionStatus Playing { get; set; }
        public int EffectiveRating { get; set; }
    }
}
