using GoalLine.Structures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoalLine.Data
{
    public class PlayerAdapter
    {
        public void AssignToTeam(int PlayerID, int TeamID)
        {
            Player p = World.Players[PlayerID];
            int OldTeam = p.CurrentTeam;

            if(OldTeam > -1)
            {
                for (int i = 0; i < World.Teams[OldTeam].PlayerIDs.Count; i++)
                {
                    if (World.Teams[OldTeam].PlayerIDs[i] == PlayerID)
                    {
                        World.Teams[OldTeam].PlayerIDs.RemoveAt(i);
                    }
                }
            }

            World.Players[PlayerID].CurrentTeam = TeamID;
            World.Teams[TeamID].PlayerIDs.Add(PlayerID);
        }
    }
}
