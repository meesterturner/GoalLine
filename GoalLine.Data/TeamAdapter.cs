using GoalLine.Structures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoalLine.Data
{
    public class TeamAdapter
    {
        public List<Player> GetPlayersInTeam(int TeamID)
        {
            List<Player> retVal = new List<Player>();
            List<int> PlayerIDs = World.Teams[TeamID].PlayerIDs;

            foreach(int PlayerID in PlayerIDs)
            {
                retVal.Add(World.Players[PlayerID]);
            }

            return retVal;
        }

        public Team GetTeam(int TeamID)
        {
            return World.Teams[TeamID];
        }

        public Team GetTeamByManager(int ManagerID)
        {
            return World.Teams[World.Managers[ManagerID].CurrentTeam];
        }

        public Team GetTeamByPlayer(int PlayerID)
        {
            return World.Teams[World.Players[PlayerID].CurrentTeam];
        }

        public int AddTeam(Team t)
        {
            int NextID = World.Teams.Count;
            t.UniqueID = NextID;
            World.Teams.Add(t);

            return NextID;
        }

        public List<Team> GetTeamsByLeague(int LeagueID)
        {
            return (from team in World.Teams
                    where team.LeagueID == LeagueID
                    select team).ToList();
        }
    }
}
