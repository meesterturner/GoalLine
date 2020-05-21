using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoalLine.Matchday
{
    public interface IMatchCallback
    {
        string Commentary { get; set; }
        MatchSegment Segment { get; set; }
        MatchEventType EventType { get; set; }
        int SegmentTimeSeconds { get; set; }
        void UpdateUI();
    }
}
