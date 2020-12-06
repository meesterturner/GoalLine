using GoalLine.Structures;
using GoalLine.Data;

namespace GoalLine.Matchday
{
    public interface IMatchCallback
    {
        string Commentary { get; set; }
        MatchStatus MatchStatus { get; set; }
        MatchEventType EventType { get; set; }

        void UpdateUI();
        void MatchStarting(Fixture f, bool Interactive);
        void MatchFinished(Fixture f, bool Interactive);
        void MatchdayComplete();
    }
}
