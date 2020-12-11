using System;
using System.Collections.Generic;
using GoalLine.Data;
using GoalLine.Processes.ProcessLogic;
using GoalLine.Structures;

namespace GoalLine.Processes
{
    class MedicalProcesses : IProcess
    {
        public void EndOfDay()
        {
            PlayerMedical pm = new PlayerMedical();
            pm.AllPlayersDailyUpdate();
        }

        public void MatchDayEnd()
        {
            throw new NotImplementedException();
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
