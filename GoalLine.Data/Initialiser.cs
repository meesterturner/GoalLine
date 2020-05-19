using System;
using System.Collections.Generic;
using System.Linq;
using GoalLine.Structures;
using GoalLine.Resources;

namespace GoalLine.Data
{
    public class Initialiser
    {
        private const int STARTYEAR = 2020;
        private const int MAXTEAMSPERLEAGUE = 22;
        private const int MAXPLAYERS = 6000;

        public void CreateWorld()
        {
            AssignDates();

            CreatePlayers();
            CreateLeagues();
            CreateTeams();
        }

        private void AssignDates()
        {
            World.CurrentDate = new DateTime(STARTYEAR, 7, 1);
            World.PreSeasonDate = World.CurrentDate;
            World.MainSeasonDate = World.PreSeasonDate.AddMonths(1);
        }

        private void CreateTeams()
        {
            

            string[] TeamFirstName = TextResources.TeamList(TeamListResource.Place, ResourceLanguage.en);
            string[] TeamLastName = TextResources.TeamList(TeamListResource.Suffix, ResourceLanguage.en);

            Dictionary<string, bool> UsedNames = new Dictionary<string, bool>();

            Utils rand = new Utils();
            LeagueAdapter la = new LeagueAdapter();
            List<League> Leagues = la.GetLeagues();

            foreach(League L in Leagues)
            {
                for (int i = 0; i <= MAXTEAMSPERLEAGUE - 1; i++)
                {
                    int r;

                    Team NewTeam = new Team();

                    while (true)
                    {
                        r = rand.RandomInclusive(0, TeamFirstName.GetUpperBound(0));
                        NewTeam.Name = TeamFirstName[r] + " ";

                        r = rand.RandomInclusive(0, TeamLastName.GetUpperBound(0));
                        NewTeam.Name = NewTeam.Name + TeamLastName[r];

                        if (!UsedNames.ContainsKey(NewTeam.Name))
                        {
                            break;
                        }

                    }

                    UsedNames.Add(NewTeam.Name, true);

                    NewTeam.ManagerID = -1;
                    NewTeam.LeagueID = L.UniqueID;

                    TeamAdapter ta = new TeamAdapter();
                    int NewID = ta.AddTeam(NewTeam);

                    AssignPlayersToTeam(NewID);
                }

            
            }
        }

        private void CreatePlayers()
        {

            string[] FirstName = TextResources.NameList(NameListResource.FirstNames, ResourceLanguage.en);
            string[] LastName = TextResources.NameList(NameListResource.LastNames, ResourceLanguage.en);


            Utils rand = new Utils();

            for (int i = 0; i <= MAXPLAYERS - 1; i++)
            {
                int r;

                Player NewPlayer = new Player();
                NewPlayer.UniqueID = i;

                r = rand.RandomInclusive(0, FirstName.GetUpperBound(0));
                NewPlayer.FirstName = FirstName[r];

                r = rand.RandomInclusive(0, 100);
                if(r < 7)
                {
                    // Double-barrelled surname
                    r = rand.RandomInclusive(0, LastName.GetUpperBound(0));
                    NewPlayer.LastName = LastName[r] + "-";

                    int r2 = r; // avoiding any Smith-Smith names
                    while(r2 == r)
                    {
                        r2 = rand.RandomInclusive(0, LastName.GetUpperBound(0));
                        if(r2 != r)
                        {
                            NewPlayer.LastName += LastName[r2];
                        }
                    }
                    
                    
                } else
                {
                    NewPlayer.LastName = NewPlayer.FirstName;

                    while(NewPlayer.LastName == NewPlayer.FirstName) // Make sure we don't get a James James situation
                    {
                        r = rand.RandomInclusive(0, LastName.GetUpperBound(0));
                        NewPlayer.LastName = LastName[r];
                    }
                }

                r = rand.RandomInclusive(1, 5);

                PlayerPosition p = PlayerPosition.None;

                switch(r)
                {
                    case 1:
                        p = PlayerPosition.Goalkeeper;
                        
                        break;

                    case 2:
                        p = PlayerPosition.Defender;
                        break;

                    case 3:
                        p = PlayerPosition.Midfielder;
                        break;

                    case 4:
                        p = PlayerPosition.Attacker;
                        break;

                    case 5:
                        p = PlayerPosition.Striker;
                        break;

                    default:
                        throw new ArgumentOutOfRangeException("Unknown player position");
                }

                NewPlayer.Position = p;
                if(p == PlayerPosition.Goalkeeper || p == PlayerPosition.Striker)
                {
                    NewPlayer.PreferenceLeft = 0;
                    NewPlayer.PreferenceRight = 0;
                    NewPlayer.PreferenceMiddle = 100;
                } else
                {
                    NewPlayer.PreferenceLeft = rand.RandomInclusive(0,100);
                    NewPlayer.PreferenceRight = rand.RandomInclusive(0, 100);
                    NewPlayer.PreferenceMiddle = rand.RandomInclusive(0, 100);

                }

                NewPlayer.Agility = rand.RandomInclusive(0, 100);
                NewPlayer.Attitude = rand.RandomInclusive(0, 100);
                NewPlayer.Speed = rand.RandomInclusive(0, 100);
                NewPlayer.Stamina = rand.RandomInclusive(0, 100);

                NewPlayer.Wages = rand.RandomInclusive(5, 80) * 100;

                NewPlayer.DateOfBirth = DateTime.Now.AddDays(rand.RandomInclusive(0 - (32 * 365), 0-(18*365)));
                NewPlayer.CurrentTeam = -1;

                PlayerAdapter pa = new PlayerAdapter();
                pa.AddPlayer(NewPlayer);
            }
        }

        private void CreateLeagues()
        {
            LeagueAdapter la = new LeagueAdapter();

            League L;

            L = new League();
            L.Name = "Fenwick League 1";
            la.AddLeague(L);

            L = new League();
            L.Name = "Fenwick League 2";
            la.AddLeague(L);
        }

        private void AssignPlayersToTeam(int TeamID)
        {
            Utils rand = new Utils();
            AssignPlayersToTeam_Worker(TeamID, PlayerPosition.Goalkeeper, PlayerPositionSide.Centre, rand.RandomInclusive(1, 3));

            AssignPlayersToTeam_Worker(TeamID, PlayerPosition.Defender, PlayerPositionSide.Left, rand.RandomInclusive(1, 3));
            AssignPlayersToTeam_Worker(TeamID, PlayerPosition.Defender, PlayerPositionSide.Centre, rand.RandomInclusive(3, 5));
            AssignPlayersToTeam_Worker(TeamID, PlayerPosition.Defender, PlayerPositionSide.Right, rand.RandomInclusive(1, 3));

            AssignPlayersToTeam_Worker(TeamID, PlayerPosition.Midfielder, PlayerPositionSide.Left, rand.RandomInclusive(1, 3));
            AssignPlayersToTeam_Worker(TeamID, PlayerPosition.Midfielder, PlayerPositionSide.Centre, rand.RandomInclusive(3, 5));
            AssignPlayersToTeam_Worker(TeamID, PlayerPosition.Midfielder, PlayerPositionSide.Right, rand.RandomInclusive(1, 3));

            AssignPlayersToTeam_Worker(TeamID, PlayerPosition.Attacker, PlayerPositionSide.Left, rand.RandomInclusive(0, 1));
            AssignPlayersToTeam_Worker(TeamID, PlayerPosition.Attacker, PlayerPositionSide.Centre, rand.RandomInclusive(1, 2));
            AssignPlayersToTeam_Worker(TeamID, PlayerPosition.Attacker, PlayerPositionSide.Right, rand.RandomInclusive(0, 1));

            AssignPlayersToTeam_Worker(TeamID, PlayerPosition.Striker, PlayerPositionSide.Centre, rand.RandomInclusive(0, 2));
        }

        private void AssignPlayersToTeam_Worker(int TeamID, PlayerPosition pos, PlayerPositionSide side, int max)
        {
            Utils rand = new Utils();

            for (int i = 1; i <= max; i++)
            {
                // Select free players of given type
                PlayerAdapter pa = new PlayerAdapter();
                List<Player> playerList = pa.GetPlayers(-1, pos, side);

                int found = playerList.Count();
                if (found == 0)
                {
                    throw new Exception("Not enough players");
                }

                int which = rand.RandomInclusive(0, found - 1);
                PlayerAdapter pad = new PlayerAdapter();

                pad.AssignToTeam(playerList[which].UniqueID, TeamID);
            }
        }

    }
}
