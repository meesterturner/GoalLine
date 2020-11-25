using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime;
using System.Text;
using System.Threading.Tasks;
using GoalLine.Data;
using GoalLine.Structures;
using GoalLine.Processes.ProcessLogic;

namespace GoalLine.Processes
{
    class AIProcesses : IProcess
    {
        public void EndOfDay()
        {
            throw new NotImplementedException();
        }

        public void MatchDayEnd()
        {
            throw new NotImplementedException(); 
        }

        public void MatchDayStart()
        {
            AITeamAndTactics ait = new AITeamAndTactics();
            ait.SelectTeamIfPlaying();
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
