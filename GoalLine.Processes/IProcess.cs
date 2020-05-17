using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoalLine.Processes
{
    public interface IProcess
    {
        void StartOfDay();
        void EndOfDay();
        void PreSeasonStart();
        void PreSeasonEnd();
        void SeasonStart();
        void SeasonEnd();
        void MatchDayStart();
        void MatchDayEnd();

    }
}
