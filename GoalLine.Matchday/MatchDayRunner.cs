using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GoalLine.Data;
using GoalLine.Structures;

namespace GoalLine.Matchday
{
    public class MatchDayRunner
    {
        /// <summary>
        /// Run all the matches for the current game date
        /// </summary>
        /// <param name="MatchCallback"></param>
        public void Run(IMatchCallback MatchCallback)
        {
            MatchPlayer mp = new MatchPlayer();
            WorldAdapter wa = new WorldAdapter();
            FixtureAdapter fa = new FixtureAdapter();
            TeamAdapter ta = new TeamAdapter();
            ManagerAdapter ma = new ManagerAdapter();

            foreach (Fixture f in fa.GetFixtures(wa.CurrentDate))
            {
                mp.Fixture = f;
                mp.Interactive = false;

                for (int t = 0; t <= 1; t++)
                {
                    if(ma.GetManager(ta.GetTeam(f.TeamIDs[t]).ManagerID).Human)
                    {
                        mp.Interactive = true;
                        break;
                    }
                }

                mp.MatchCallback = MatchCallback;
                mp.StartMatch();
            }

            MatchCallback.MatchdayComplete();
        }
    }
}
