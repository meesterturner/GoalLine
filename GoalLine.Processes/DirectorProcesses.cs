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
    class DirectorProcesses : IProcess
    {
        public void EndOfDay()
        {
            throw new NotImplementedException();
        }

        public void MatchDayEnd()
        {
            DirectorLogic dl = new DirectorLogic();
            List<Fixture> fixtures = new FixtureAdapter().GetFixtures(new WorldAdapter().CurrentDate);
            dl.GoodAndBadFixtureFeedback(fixtures);
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
            throw new NotImplementedException();
        }

        public void SeasonEnd()
        {
            throw new NotImplementedException();
        }

        public void SeasonStart()
        {
            throw new NotImplementedException();
        }

        public void StartOfDay()
        {
            throw new NotImplementedException();
        }
    }
}
