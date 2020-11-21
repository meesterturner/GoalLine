using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GoalLine.Data;
using GoalLine.Structures;

namespace GoalLine.Processes
{
    class SeasonProcesses : IProcess
    {
        public void EndOfDay()
        {
            throw new NotImplementedException();
        }

        public void MatchDayEnd()
        {
            List<Fixture> fixtures = new FixtureAdapter().GetFixtures(new WorldAdapter().CurrentDate);
            UpdateMatchStats(fixtures);
        }

        public void MatchDayStart()
        {
            throw new NotImplementedException();
        }

        public void PreSeasonEnd()
        {
            throw new NotImplementedException();
        }

        public void PreSeasonStart()
        {
            CreateSeasonFixtures();
        }

        public void SeasonEnd()
        {
            throw new NotImplementedException();
        }

        public void SeasonStart()
        {
            ResetSeasonStatistics();
        }

        public void StartOfDay()
        {
            throw new NotImplementedException();
        }

        private void CreateSeasonFixtures()
        {
            // First find first available Saturday
            WorldAdapter wa = new WorldAdapter();
            DateTime FirstMatchDate = wa.MainSeasonDate;
            while(FirstMatchDate.DayOfWeek != DayOfWeek.Saturday)
            {
                FirstMatchDate = FirstMatchDate.AddDays(1);
            }

            FixtureAdapter fa = new FixtureAdapter();
            fa.ClearFixtures();

            LeagueAdapter la = new LeagueAdapter();
            List<League> Leagues = la.GetLeagues();

            foreach(League L in Leagues)
            {
                // Get teams in the league we're looking at.
                TeamAdapter ta = new TeamAdapter();
                List<Team> TeamList = ta.GetTeamsByLeague(L.UniqueID);

                // Create that league's fixtures (Circle Method)
                // https://en.wikipedia.org/wiki/Round-robin_tournament

                int NumberOfTeams = TeamList.Count();
                int Half = NumberOfTeams / 2;

                int[] TeamIDs = new int[NumberOfTeams + 1]; // Create a grid with an additional blank space to allow rotation
                for(int i = 0; i < TeamList.Count(); i++)
                {
                    TeamIDs[i] = TeamList[i].UniqueID;
                }
                TeamIDs[NumberOfTeams] = -999; // For debugging

                DateTime MatchDate = FirstMatchDate;
                
                for(int HomeAwayLeg = 1; HomeAwayLeg <= 2; HomeAwayLeg++)
                {
                    bool homeFirst = (HomeAwayLeg == 1);

                    for (int wk = 1; wk <= NumberOfTeams - 1; wk++)
                    {
                        // Make the week's fixtures
                        for (int gridpos = 0; gridpos < Half; gridpos++)
                        {
                            Fixture f = new Fixture();
                            f.Date = MatchDate;
                            f.LeagueID = L.UniqueID;
                            if(homeFirst)
                            {
                                f.TeamIDs[0] = TeamIDs[gridpos];
                                f.TeamIDs[1] = TeamIDs[gridpos + Half];
                            } else
                            {
                                f.TeamIDs[1] = TeamIDs[gridpos];
                                f.TeamIDs[0] = TeamIDs[gridpos + Half];
                            }
                            
                            fa.AddFixture(f);
                        }

                        // Rotate the grid! (Clockwise, as opposed to the description in the wiki link)
                        // Top "row" (first half) need to move L to R
                        // Bottom "row" (second half) need to move R to L
                        // Bottom left cell moves into position 1 and position 0 doesn't move
                        // Comments below refer to 16 teams, grid positions 0-7 (top), 8-15 (bottom), 16 spare.

                        TeamIDs[NumberOfTeams] = TeamIDs[Half - 1]; // Top right cell to bottom row, spare cell
                        for (int i = Half - 2; i >= 1; i--) // Move top row, except cell zero and rightmost cell, right one
                        {
                            TeamIDs[i + 1] = TeamIDs[i];
                        }
                        TeamIDs[1] = TeamIDs[Half]; // Bottom left cell to position 1
                        for (int i = Half; i <= NumberOfTeams - 1; i++) // Shift entire bottom row (including spare, excluding bottom left) left by one cell
                        {
                            TeamIDs[i] = TeamIDs[i + 1];
                        }

                        // Go to next date
                        MatchDate = MatchDate.AddDays(7);
                        homeFirst = !homeFirst;
                    }
                }
                
            }
        }

        private void ResetSeasonStatistics()
        {
            TeamAdapter ta = new TeamAdapter();
            List<Team> teams = ta.GetTeams();

            foreach(Team t in teams)
            {
                ta.UpdateTeamSeasonStatistics(t.UniqueID, new TeamStats());
            }
        }

        private void UpdateMatchStats(List<Fixture> fixtures)
        {
            const int POINTS_WON = 3;
            const int POINTS_DRAWN = 1;

            TeamAdapter ta = new TeamAdapter();

            foreach(Fixture fixture in fixtures)
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
    }
}
