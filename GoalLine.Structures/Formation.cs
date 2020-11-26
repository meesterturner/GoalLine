using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace GoalLine.Structures
{
    public class Formation
    {
        public int UniqueID { get; set; }
        public string Name { get; set; }
        public List<Point2> Points { get; set; }
        public bool System { get; set; }
    }

    public class SuitablePlayerInfo
    {
        public List<PlayerPosition> Positions { get; set; }
        public List<PlayerPositionSide> Sides { get; set; }
    }
}
