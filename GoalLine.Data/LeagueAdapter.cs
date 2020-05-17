using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GoalLine.Structures;

namespace GoalLine.Data
{
    public class LeagueAdapter
    {
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

            foreach(DateTime d in Dates)
            {
                if(d.Date != LastDate)
                {
                    retVal.Add(d);
                    LastDate = d;
                }
            }

            return retVal;
        }

        public DateTime GetNextDateForLeagueMatches(int LeagueID, bool AllowCurrentDate)
        {
            DateTime testDate = World.CurrentDate;
            if(!AllowCurrentDate)
            {
                testDate = testDate.AddDays(1);
            }
            return  (from f in World.Fixtures
                    where f.LeagueID == LeagueID && f.Date >= testDate
                     orderby f.Date
                    select f.Date).FirstOrDefault();
        }

        public List<Fixture> GetFixturesForLeagueForDate(int LeagueID, DateTime date)
        {
            IEnumerable<Fixture> fixtures =
                    from f in World.Fixtures
                    where f.LeagueID == LeagueID && f.Date == date
                    orderby World.Teams[f.TeamIDs[0]].Name, World.Teams[f.TeamIDs[1]].Name
                    select f;

            return fixtures.ToList();
        }
    }
}
