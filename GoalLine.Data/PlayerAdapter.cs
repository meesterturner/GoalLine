﻿using GoalLine.Structures;
using System;
using System.Collections.Generic;
using System.Linq;
using GoalLine.Resources;

namespace GoalLine.Data
{
    public class PlayerAdapter
    {
        public void AssignToTeam(int PlayerID, int TeamID)
        {
            AssignToTeam(PlayerID, TeamID, false);
        }

        public void AssignToTeam(int PlayerID, int TeamID, bool FromNewGame)
        {
            Player p = World.Players[PlayerID];
            int OldTeam = p.CurrentTeam;

            if(OldTeam > -1)
            {
                if(World.Teams[OldTeam].Players.ContainsKey(PlayerID))
                {
                    World.Teams[OldTeam].Players.Remove(PlayerID);
                }
            }

            World.Players[PlayerID].CurrentTeam = TeamID;

            World.Teams[TeamID].Players.Add(PlayerID, new TeamPlayer(PlayerID));

            if(!FromNewGame)
            {
                if(OldTeam > -1)
                {
                    SetUseInitialOnPlayers(OldTeam);
                }
                SetUseInitialOnPlayers(TeamID);
            }
            
        }

        public void SetUseInitialOnPlayers(int TeamID)
        {
            Dictionary<int, TeamPlayer> PlayerIDs = World.Teams[TeamID].Players;

            // Reset all to start with
            foreach(KeyValuePair<int, TeamPlayer> CurPlayer in PlayerIDs) {
                World.Players[CurPlayer.Key].UseInitial = false;
            }

            // Go through each one
            foreach (KeyValuePair<int, TeamPlayer> CurPlayer in PlayerIDs)
            {
                // Reset to False initially
                World.Players[CurPlayer.Key].UseInitial = false;

                string LastName = World.Players[CurPlayer.Key].LastName.ToUpper();

                // Go through all players again
                foreach (KeyValuePair<int, TeamPlayer> TestPlayer in PlayerIDs)
                {
                    // If not the current player in first loop, and surname matches, flag as true
                    if(TestPlayer.Key != CurPlayer.Key && LastName == World.Players[TestPlayer.Key].LastName.ToUpper())
                    {
                        World.Players[CurPlayer.Key].UseInitial = true;
                        break;
                    }
                }
            }
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

        public List<Player> GetPlayers()
        {
            return World.Players;
        }

        public List<Player> GetPlayers(int TeamID)
        {
            return (from player in World.Players
                    where player.CurrentTeam == TeamID
                    select player).ToList();
        }

        public List<Player> GetPlayers(int TeamID, PlayerPosition pos, PlayerPositionSide side)
        {
            return (from player in World.Players
                    where player.CurrentTeam == TeamID &&
                          player.Position == pos &&
                          player.PreferredSide == side
                    select player).ToList();
        }

        public List<Player> GetPlayers(int TeamID, PlayerPosition pos, PlayerPositionSide side, int MinEffectiveness, int MaxEffectiveness)
        {
            return (from player in World.Players
                    where player.CurrentTeam == TeamID &&
                          player.Position == pos &&
                          player.PreferredSide == side &&
                          player.OverallRating >= MinEffectiveness &&
                          player.OverallRating <= MaxEffectiveness
                         
                    select player).ToList();
        }

        public string PositionAndSideText(Player p, bool Short)
        {
            string pos = LangResources.CurLang.PositionsList[p.PositionInt];
            string side = "";

            if(Short)
            {
                pos = pos.Substring(0, 1);
            }

            if (p.Position != PlayerPosition.Goalkeeper && p.Position != PlayerPosition.Striker)
            {
                side = " " + LangResources.CurLang.SidesList[p.PreferredSideInt];
                if (Short)
                {
                    side = side.Substring(0, 2);
                }
            }

            return pos + side;
        }

        public void UpdatePlayer(Player p)
        {
            World.Players[p.UniqueID] = p;
        }
    }
}
