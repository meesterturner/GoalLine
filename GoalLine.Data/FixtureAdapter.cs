using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GoalLine.Structures;

namespace GoalLine.Data
{
    public class FixtureAdapter
    {
        public void ClearFixtures()
        {
            World.Fixtures = null;
            World.Fixtures = new List<Fixture>();
        }

        public int AddFixture(Fixture f)
        {
            int NextID = World.Fixtures.Count;
            f.UniqueID = NextID;
            World.Fixtures.Add(f);

            return NextID;
        }

        public Fixture GetFixture(int FixtureID)
        {
            return World.Fixtures[FixtureID];
        }

        public List<DateTime> GetDistinctDatesForLeagueMatches(int LeagueID)
        {
            List<DateTime> retVal = new List<DateTime>();

            IEnumerable<DateTime> Dates =
                    from f in World.Fixtures
                    where f.LeagueID == LeagueID
                    orderby f.Date
                    select f.Date;

            // TODO: Should be able to do this better, I'm sure....!! 
            DateTime LastDate = new DateTime(1979, 11, 06);

            foreach (DateTime d in Dates)
            {
                if (d.Date != LastDate)
                {
                    retVal.Add(d);
                    LastDate = d;
                }
            }

            return retVal;
        }

        /// <summary>
        /// Returns true/false depending on if there are any matches today
        /// </summary>
        /// <returns>True: Matches today, False: no matches today</returns>
        public bool IsTodayAMatchDay()
        {
            DateTime today = new WorldAdapter().CurrentDate;

            DateTime Found = (from f in World.Fixtures
                              where f.Date == today
                              orderby f.Date
                              select f.Date).FirstOrDefault();
            
            return (Found.Year > 1);
        }

        /// <summary>
        /// Returns true/false depending on if there are any matches today for the specified team
        /// </summary>
        /// <param name="TeamID"></param>
        /// <returns></returns>
        public bool IsTodayAMatchDay(int TeamID)
        {
            DateTime today = new WorldAdapter().CurrentDate;

            DateTime Found = (from f in World.Fixtures
                              where f.Date == today && (f.TeamIDs[0] == TeamID || f.TeamIDs[1] == TeamID)
                              orderby f.Date
                              select f.Date).FirstOrDefault();

            return (Found.Year > 1);
        }

        public DateTime GetNextDateForLeagueMatches(int LeagueID, bool AllowCurrentDate)
        {
            DateTime testDate = World.WorldState.CurrentDate;
            if (!AllowCurrentDate)
            {
                testDate = testDate.AddDays(1);
            }
            return (from f in World.Fixtures
                    where f.LeagueID == LeagueID && f.Date >= testDate
                    orderby f.Date
                    select f.Date).FirstOrDefault();
        }

        public List<Fixture> GetFixtures(DateTime date)
        {
            IEnumerable<Fixture> fixtures =
                    from f in World.Fixtures
                    where f.Date == date
                    orderby World.Teams[f.TeamIDs[0]].Name, World.Teams[f.TeamIDs[1]].Name
                    select f;

            return fixtures.ToList();
        }


        public List<Fixture> GetFixtures(int LeagueID, DateTime date)
        {
            IEnumerable<Fixture> fixtures =
                    from f in World.Fixtures
                    where f.LeagueID == LeagueID && f.Date == date
                    orderby World.Teams[f.TeamIDs[0]].Name, World.Teams[f.TeamIDs[1]].Name
                    select f;

            return fixtures.ToList();
        }

        public List<Fixture> GetFixtures(int TeamID)
        {
            IEnumerable<Fixture> fixtures =
                    from f in World.Fixtures
                    where f.TeamIDs[0] == TeamID || f.TeamIDs[1] == TeamID
                    orderby f.Date
                    select f;

            return fixtures.ToList();
        }

        public Fixture GetNextFixture(int TeamID, DateTime date)
        {
            List<Fixture> all = GetFixtures(TeamID);

            return (from f in all
                    where f.Date >= date && f.Played == false
                    orderby f.UniqueID
                    select f).FirstOrDefault();
        }

        public void UpdateFixture(Fixture f)
        {
            int id = f.UniqueID;
            World.Fixtures[id] = f;
        }
    }
}
