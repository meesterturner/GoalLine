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
            throw new NotImplementedException();
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
            throw new NotImplementedException();
        }

        public void StartOfDay()
        {
            throw new NotImplementedException();
        }

        private void CreateSeasonFixtures()
        {
            // First find first available Saturday
            DateTime FirstMatchDate = World.MainSeasonDate;
            while(FirstMatchDate.DayOfWeek != DayOfWeek.Saturday)
            {
                FirstMatchDate = FirstMatchDate.AddDays(1);
            }

            World.Fixtures = null;

            FixtureAdapter fa = new FixtureAdapter();
            fa.ClearFixtures();

            foreach(League L in World.Leagues)
            {
                // Get teams in the league we're looking at.
                List<int> TeamList =
                    (from team in World.Teams
                    where team.LeagueID == L.UniqueID
                    select team.UniqueID).ToList();

                // Create that league's fixtures
                // https://en.wikipedia.org/wiki/Round-robin_tournament

                int NumberOfTeams = TeamList.Count();
                int[] TeamIDs = new int[NumberOfTeams + 1]; // Create a grid with an additional blank space to allow rotation
                for(int i = 0; i < TeamList.Count(); i++)
                {
                    TeamIDs[i] = TeamList[i];
                }

                DateTime MatchDate = FirstMatchDate;
                
                for(int HomeAwayLeg = 1; HomeAwayLeg <= 2; HomeAwayLeg++)
                {
                    bool homeFirst = (HomeAwayLeg == 1);

                    for (int wk = 1; wk <= NumberOfTeams - 1; wk++)
                    {
                        // Make the week's fixtures
                        for (int gridpos = 0; gridpos < (NumberOfTeams / 2); gridpos++)
                        {
                            Fixture f = new Fixture();
                            f.Date = MatchDate;
                            f.LeagueID = L.UniqueID;
                            if(homeFirst)
                            {
                                f.TeamIDs[0] = TeamIDs[gridpos];
                                f.TeamIDs[1] = TeamIDs[gridpos + (NumberOfTeams / 2)];
                            } else
                            {
                                f.TeamIDs[1] = TeamIDs[gridpos];
                                f.TeamIDs[0] = TeamIDs[gridpos + (NumberOfTeams / 2)];
                            }
                            
                            fa.AddFixture(f);
                        }

                        // Rotate the grid! (Counterclockwise, though - fix later)
                        for (int i = NumberOfTeams - 1; i >= 1; i--)
                        {
                            TeamIDs[i + 1] = TeamIDs[i];
                        }
                        TeamIDs[1] = TeamIDs[NumberOfTeams]; // Move last to almost first

                        MatchDate = MatchDate.AddDays(7);
                        homeFirst = !homeFirst;
                    }
                }
                
            }
        }
    }
}
