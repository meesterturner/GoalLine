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
    }
}
