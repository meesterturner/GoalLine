using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GoalLine.Data;
using GoalLine.Structures;

namespace GoalLine.Processes.ProcessLogic
{
    class PlayerMedical
    {
        public void AllPlayersDailyUpdate()
        {
            const double standardDailyIncrement = 7;

            PlayerAdapter pa = new PlayerAdapter();
            TeamAdapter ta = new TeamAdapter();
            FixtureAdapter fa = new FixtureAdapter();
            List<Player> players = pa.GetPlayers();

            for(int i = 0; i < players.Count; i++)
            {
                Player p = players[i];

                if(p.Health < 100)
                {
                    // If player was not in a game today, then we increase their health
                    bool doUpdate = true;

                    if(p.CurrentTeam != -1)
                    {
                        if(ta.GetPlayerSelectionStatus(p.UniqueID, p.CurrentTeam, false) != PlayerSelectionStatus.None)
                        {
                            if(fa.IsTodayAMatchDay(p.CurrentTeam))
                            {
                                doUpdate = false;
                            }
                        }
                    }


                    if(doUpdate)
                    {
                        double increment = standardDailyIncrement * ((double)p.Fitness / 100);
                        if (increment < 1)
                            increment = 1;

                        p.Health += increment;

                        if (p.Health > 100)
                            p.Health = 100;

                        pa.UpdatePlayer(p);
                    }
                    
                }
            }
        }
    }
}
