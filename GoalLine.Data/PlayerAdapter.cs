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

        public Player GetPlayer(int PlayerID)
        {
            return World.Players[PlayerID];
        }

        public int AddPlayer(Player p)
        {
            int NextID = World.Players.Count;
            p.UniqueID = NextID;
            World.Players.Add(p);

            return NextID;
        }

        public List<Player> GetPlayers(int TeamID, PlayerPosition pos, PlayerPositionSide side)
        {
            return (from player in World.Players
                    where player.CurrentTeam == -1 &&
                          player.Position == pos &&
                          player.PreferredSide == side
                    select player).ToList();
        }
    }
}
