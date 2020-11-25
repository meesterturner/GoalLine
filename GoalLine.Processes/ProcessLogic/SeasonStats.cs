using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GoalLine.Data;
using GoalLine.Structures;

namespace GoalLine.Processes.ProcessLogic
{
    class SeasonStats
    {
        /// <summary>
        /// Loop round all teams, updating the last known selections. This is so the human manager can see the last known starting pick for the AI teams.
        /// </summary>
        public void UpdateLastKnownPicks()
        {
            TeamAdapter ta = new TeamAdapter();
            List<Team> Teams = ta.GetTeams();

            foreach (Team T in Teams)
            {
                ta.UpdateLastKnownPick(T.UniqueID);
            }
        }

        /// <summary>
        /// Updates the team statistics from the fixtures/matches that have just been played
        /// </summary>
        /// <param name="fixtures">List of fixture objects to update</param>
        public void UpdateMatchStats(List<Fixture> fixtures)
        {
            const int POINTS_WON = 3;
            const int POINTS_DRAWN = 1;

            TeamAdapter ta = new TeamAdapter();

            foreach (Fixture fixture in fixtures)
            {
                TeamStats[] stats = new TeamStats[2];

                for (int t = 0; t <= 1; t++)
                {
                    stats[t] = ta.GetTeam(fixture.TeamIDs[t]).SeasonStatistics;
                    stats[t].GamesPlayed++;
                    stats[t].GoalsScored += fixture.Score[t];
                    stats[t].GoalsConceded += fixture.Score[1 - t];
                }

                if (fixture.Score[0] == fixture.Score[1])
                {
                    for (int t = 0; t <= 1; t++)
                    {
                        stats[t].Drawn += 1;
                        stats[t].Points += POINTS_DRAWN;
                    }
                }
                else
                {
                    int WinningTeam = -1;
                    int LosingTeam = -1;

                    if (fixture.Score[0] > fixture.Score[1])
                    {
                        WinningTeam = 0;
                        LosingTeam = 1;
                    }
                    else
                    {
                        WinningTeam = 1;
                        LosingTeam = 0;
                    }

                    stats[WinningTeam].Won += 1;
                    stats[LosingTeam].Lost += 1;

                    stats[WinningTeam].Points += POINTS_WON;
                }

                for (int t = 0; t <= 1; t++)
                {
                    ta.UpdateTeamSeasonStatistics(fixture.TeamIDs[t], stats[t]);
                }
            }


        }

        /// <summary>
        /// Reset the Season Stats for all teams
        /// </summary>
        public void ResetSeasonStatistics()
        {
            TeamAdapter ta = new TeamAdapter();
            List<Team> teams = ta.GetTeams();

            foreach (Team t in teams)
            {
                ta.UpdateTeamSeasonStatistics(t.UniqueID, new TeamStats());
            }
        }
    }
}
