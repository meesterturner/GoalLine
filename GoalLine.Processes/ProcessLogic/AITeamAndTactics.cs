using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GoalLine.Data;
using GoalLine.Structures;

namespace GoalLine.Processes.ProcessLogic
{
    class AITeamAndTactics
    {
        private class AvailablePlayer
        {
            public int PlayerID { get; set; }
            public PlayerPosition Pos { get; set; }
            public PlayerPositionSide Side { get; set; }
            public int Rating { get; set; }
        }

        /// <summary>
        /// Return a list of all eligible players for this match
        /// </summary>
        /// <param name="t">Team object</param>
        /// <param name="f">Fixture object</param>
        /// <returns>List of </returns>
        private List<AvailablePlayer> GetEligiblePlayers(Team t, Fixture f)
        {
            List<AvailablePlayer> retVal = new List<AvailablePlayer>();
            PlayerAdapter pa = new PlayerAdapter();

            List<Player> players = pa.GetPlayers(t.UniqueID);

            foreach (Player p in players)
            {
                // TODO: When we have injuries, remember to deal with these appropriately.
                AvailablePlayer ap = new AvailablePlayer();
                ap.PlayerID = p.UniqueID;
                ap.Pos = p.Position;
                ap.Side = p.PreferredSide;
                ap.Rating = p.OverallRating;

                retVal.Add(ap);
            }

            return retVal;
        }

        /// <summary>
        /// Loop round all AI teams, and select a team appropriate to play the fixture.
        /// </summary>
        public void SelectTeamIfPlaying()
        {
            FixtureAdapter fa = new FixtureAdapter();
            WorldAdapter wa = new WorldAdapter();
            TeamAdapter ta = new TeamAdapter();
            ManagerAdapter ma = new ManagerAdapter();

            List<Fixture> AllFixtures = fa.GetFixtures(wa.CurrentDate);

            int TESTf = 0;

            foreach (Fixture f in AllFixtures)
            {
                TESTf++;
                Debug.Print("Fixture " + f.ToString());

                for (int t = 0; t <= 1; t++)
                {
                    Team ThisTeam = ta.GetTeam(f.TeamIDs[t]);
                    Manager M = ma.GetManager(ThisTeam.ManagerID);


                    if (!M.Human)
                    {
                        Team Opposition = ta.GetTeam(f.TeamIDs[1 - t]);
                        PlayerAdapter pa = new PlayerAdapter();
                        int[,] PlayerGridPositions = new int[5, 8]; // TODO: Maybe not hard code these...

                        Formation TeamFormation = new FormationAdapter().GetFormation(ThisTeam.CurrentFormation);
                        List<AvailablePlayer> avail = GetEligiblePlayers(ThisTeam, f);

                        foreach (Point2 point in TeamFormation.Points)
                        {
                            AvailablePlayer SelPlayer = FindBestPlayerForPosition(point, avail);
                            if(SelPlayer == null)
                            {
                                throw new Exception("Unable to find a player for this position");
                            }

                            PlayerGridPositions[point.X, point.Y] = SelPlayer.PlayerID;
                            avail.Remove(SelPlayer);
                        }

                        ta.SavePlayerFormation(ThisTeam.UniqueID, TeamFormation.UniqueID, PlayerGridPositions);

                    }
                }
            }
        }

        private AvailablePlayer FindBestPlayerForPosition(Point2 GridPos, List<AvailablePlayer> avail)
        {
            PlayerAdapter pa = new PlayerAdapter();
            FormationAdapter fa = new FormationAdapter();

            SuitablePlayerInfo suit = fa.SuitablePlayerPositions(GridPos);
            AvailablePlayer best = null;

            foreach(PlayerPosition pos in suit.Positions)
            {
                foreach(PlayerPositionSide side in suit.Sides)
                {
                    best = (from ap in avail
                            where ap.Side == side && ap.Pos == pos
                            select ap).FirstOrDefault();

                    if (best != null)
                        break;
                }

                if(best != null)
                    break;
            }

            if(best == null)
            {
                best = (from ap in avail
                        orderby ap.Rating descending
                        select ap).FirstOrDefault();
            }

            return best;
        }
    }
}
