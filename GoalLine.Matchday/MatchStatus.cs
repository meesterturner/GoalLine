using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoalLine.Matchday
{
    public class MatchStatus
    {
        public MatchSegment Segment { get; set; }
        public int SegmentTimeSeconds { get; set; }
        public int MatchTimeSeconds { get; set; }
        public int TotalTime { get; set; }
        public int[] Score { get; set; } = new int[2];
        public int PossessionTeam { get; set; }
        public List<PlayerStatus>[] PlayerStatuses { get; set; } = new List<PlayerStatus>[2];
        public TacticEvaluation[] Evaluation { get; set; } = new TacticEvaluation[2];
        public int[] OverallPlayerEffectiveRating { get; set; } = new int[2];
        public int BallX { get; set; }
        //public MatchEventType MostRecentEvent { get; set; }

    }
}
