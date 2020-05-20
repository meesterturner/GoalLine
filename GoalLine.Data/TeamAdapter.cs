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

            foreach(KeyValuePair<int, TeamPlayer> tp in World.Teams[TeamID].Players)
            {
                retVal.Add(World.Players[tp.Key]);
            }

            return retVal;
        }

        public Dictionary<int, TeamPlayer> GetTeamPlayerSelections(int TeamID)
        {
            return World.Teams[TeamID].Players;
        }

        public void SetTeamPlayerSelection(int TeamID, int PlayerID, PlayerSelectionStatus Status)
        {
            World.Teams[TeamID].Players[PlayerID].Selected = Status;
        }

        public PlayerSelectionStatus CycleTeamPlayerSelection(int TeamID, int PlayerID)
        {
            PlayerSelectionStatus Old = World.Teams[TeamID].Players[PlayerID].Selected;
            PlayerSelectionStatus New;
            if (Old == PlayerSelectionStatus.None)
            {
                New = PlayerSelectionStatus.Starting;
            } else if (Old == PlayerSelectionStatus.Starting)
            {
                New = PlayerSelectionStatus.Sub;
            } else if (Old == PlayerSelectionStatus.Sub)
            {
                New = PlayerSelectionStatus.None;
            } else
            {
                throw new NotImplementedException();
            }

            SetTeamPlayerSelection(TeamID, PlayerID, New);

            return New;
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
