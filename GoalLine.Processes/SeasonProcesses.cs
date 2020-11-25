using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GoalLine.Data;
using GoalLine.Processes.ProcessLogic;
using GoalLine.Structures;

namespace GoalLine.Processes
{
    class SeasonProcesses : IProcess
    {
        public void EndOfDay()
        {
            throw new NotImplementedException();
        }

        public void MatchDayEnd()
        {
            SeasonStats ss = new SeasonStats();
            ss.UpdateLastKnownPicks();

            List<Fixture> fixtures = new FixtureAdapter().GetFixtures(new WorldAdapter().CurrentDate);
            ss.UpdateMatchStats(fixtures);
        }

        public void MatchDayStart()
        {
            throw new NotImplementedException();
        }

        public void PreSeasonEnd()
        {
            throw new NotImplementedException();
        }

        public void PreSeasonStart()
        {
            SeasonFixtures sf = new SeasonFixtures();
            sf.CreateSeasonFixtures();
        }

        public void SeasonEnd()
        {
            throw new NotImplementedException();
        }

        public void SeasonStart()
        {
            SeasonStats ss = new SeasonStats();
            ss.ResetSeasonStatistics();
        }

        public void StartOfDay()
        {
            throw new NotImplementedException();
        }
    }
}
