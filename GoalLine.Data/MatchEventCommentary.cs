using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoalLine.Data
{
    public class MatchEventCommentary
    {
        public MatchEventType EventType { get; set; }
        public string RawText { get; set; }
        public MatchSegment Segment { get; set; }

        public MatchEventCommentary()
        {

        }

        public MatchEventCommentary(MatchEventType EventType, string RawText, MatchSegment Segment)
        {
            this.EventType = EventType;
            this.RawText = RawText;
            this.Segment = Segment;
        }

    }
}
