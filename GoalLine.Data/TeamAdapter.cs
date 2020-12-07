using GoalLine.Structures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Security.Cryptography.X509Certificates;
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

        public double AveragePlayerRating(int TeamID)
        {
            double retVal = 0f;

            List<Player> players = GetPlayersInTeam(TeamID);
            int TotalRating = 0;

            foreach (Player p in players)
            {
                TotalRating += p.OverallRating;
            }
                

            if(players.Count != 0) 
            {
                retVal = (double)TotalRating / players.Count;
            }

            return retVal;
        }

        public Dictionary<int, TeamPlayer> GetTeamPlayerSelections(int TeamID)
        {
            return GetTeamPlayerSelections(TeamID, true);
        }

        public int CountSelectedPlayers(int TeamID)
        {
            int retVal = 0;

            Dictionary<int, TeamPlayer> sel = GetTeamPlayerSelections(TeamID, true);
            foreach(KeyValuePair<int, TeamPlayer> kvp in sel)
            {
                if (kvp.Value.PlayerGridX > -1 && kvp.Value.PlayerGridY > -1)
                    retVal++;
            }

            return retVal;
        }

        public Dictionary<int, TeamPlayer> GetTeamPlayerSelections(int TeamID, bool CurrentSelection)
        {
            if(CurrentSelection)
            {
                return World.Teams[TeamID].Players;
            }
            else
            {
                return World.Teams[TeamID].LastKnownPick;
            }
            
        }


        [Obsolete("Use new functions instead")]
        public void SetTeamPlayerSelection(int TeamID, int PlayerID, PlayerSelectionStatus Status)
        {
            World.Teams[TeamID].Players[PlayerID].Selected = Status;
        }

        public void SetTeamPlayerStarting(int TeamID, int PlayerID, int GridX, int GridY)
        {
            TeamPlayer tp = new TeamPlayer();
            tp.PlayerID = PlayerID;
            tp.Selected = PlayerSelectionStatus.Starting;
            tp.PlayerGridX = GridX;
            tp.PlayerGridY = GridY;
            World.Teams[TeamID].Players[PlayerID] = tp;
        }

        public void SetTeamPlayerDeselected(int TeamID, int PlayerID)
        {
            TeamPlayer tp = new TeamPlayer();
            tp.PlayerID = PlayerID;
            tp.Selected = PlayerSelectionStatus.None;
            World.Teams[TeamID].Players[PlayerID] = tp;
        }

        public void SetTeamPlayerSub(int TeamID, int PlayerID, int SubSequence)
        {
            TeamPlayer tp = new TeamPlayer();
            tp.PlayerID = PlayerID;
            tp.Selected = PlayerSelectionStatus.Sub;
            tp.SubSequence = SubSequence;
            World.Teams[TeamID].Players[PlayerID] = tp;
            
        }

        public void UpdateLastKnownPick(int TeamID)
        {
            World.Teams[TeamID].LastKnownPick = World.Teams[TeamID].Players;
            World.Teams[TeamID].LastKnownFormation = World.Teams[TeamID].CurrentFormation;
        }

        public Team GetTeam(int TeamID)
        {
            return World.Teams[TeamID];
        }

        public Team GetTeamByManager(int ManagerID)
        {
            int CurTeam = World.Managers[ManagerID].CurrentTeam;
            if(CurTeam > -1)
            {
                return World.Teams[CurTeam];
            } else
            {
                return null;
            }
            
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

        public List<Team> GetTeams()
        {
            return World.Teams;
        }

        public void UpdateTeamSeasonStatistics(int TeamID, TeamStats SeasonStatistics)
        {
            World.Teams[TeamID].SeasonStatistics = SeasonStatistics;
        }

        public void SavePlayerFormation(int TeamID, int FormationID, int[,] PlayerGridPositions)
        {
            List<int> PlayerIDs = World.Teams[TeamID].Players.Keys.ToList();
            World.Teams[TeamID].CurrentFormation = FormationID;

            foreach(int PlayerID in PlayerIDs)
            {
                SetTeamPlayerDeselected(TeamID, PlayerID);
            }

            for (int x = 0; x <= PlayerGridPositions.GetUpperBound(0); x++)
            {
                for (int y = 0; y <= PlayerGridPositions.GetUpperBound(1); y++)
                {
                    int PlayerID = PlayerGridPositions[x, y];
                    if(PlayerID > -1)
                    {
                        SetTeamPlayerStarting(TeamID, PlayerID, x, y);
                    }
                }
            }
        }
    }
}
