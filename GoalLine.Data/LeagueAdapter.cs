using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using GoalLine.Structures;

namespace GoalLine.Data
{
    public class LeagueAdapter
    {
        public int AddLeague(League l)
        {
            int NextID = World.Leagues.Count;
            l.UniqueID = NextID;
            World.Leagues.Add(l);

            return NextID;
        }

        public List<League> GetLeagues()
        {
            return World.Leagues;
        }

        public League GetLeague(int LeagueID)
        {
            return World.Leagues[LeagueID];
        }
    }
}
