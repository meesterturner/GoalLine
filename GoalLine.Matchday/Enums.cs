using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoalLine.Matchday
{
    public enum MatchEventType
    {
        None,
        Goal,
        GoalKick,
        Save,
        Miss,
        Foul,
        FreeKick,
        HalfTime,
        FullTime,
        KickOff
    }

    public enum MatchSegment
    {
        None,
        FirstHalf,
        SecondHalf,
        ExtraTimeFirstHalf,
        ExtraTimeSecondHalf,
        Penalties
    }
}
