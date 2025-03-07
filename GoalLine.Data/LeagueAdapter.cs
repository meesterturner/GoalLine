﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using GoalLine.Structures;

namespace GoalLine.Data
{
    public class LeagueAdapter
    {
        public int AddLeague(League l)
        {
            int NextID = World.Leagues.Count;
            l.UniqueID = NextID;
            World.Leagues.Add(l);

            return NextID;
        }

        public List<League> GetLeagues()
        {
            return World.Leagues;
        }

        public League GetLeague(int LeagueID)
        {
            return World.Leagues[LeagueID];
        }

        /// <summary>
        /// Returns a fairly nice to use league table, transposing the most important
        /// data into a simple format
        /// </summary>
        /// <param name="LeagueID"></param>
        /// <returns></returns>
        public List<LeagueTableRecord> LeagueTable(int LeagueID)
        {
            List<LeagueTableRecord> Records = new List<LeagueTableRecord>();

            TeamAdapter ta = new TeamAdapter();
            foreach(Team t in ta.GetTeamsByLeague(LeagueID))
            {
                LeagueTableRecord ltr = new LeagueTableRecord();
                ltr.TeamID = t.UniqueID;
                ltr.Name = t.Name;
                ltr.SeasonStatistics = t.SeasonStatistics;

                Records.Add(ltr);
                ltr = null;
            }

            List<LeagueTableRecord> retVal = (from ltr in Records
                                              orderby ltr.SeasonStatistics.Points descending, 
                                                      ltr.SeasonStatistics.GoalDifference descending,
                                                      ltr.SeasonStatistics.GoalsScored descending,
                                                      ltr.Name ascending
                                              select ltr).ToList();


            return retVal;
        }
    }
}
