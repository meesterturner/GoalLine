using System;
using System.Collections.Generic;
using System.Linq;
using GoalLine.Structures;
using GoalLine.Resources;
using System.Drawing;

namespace GoalLine.Data
{
    public class Initialiser
    {
        private const int STARTYEAR = 2020;
        private const int MAXTEAMSPERLEAGUE = 22;
        private const int MAXPLAYERS = 15000;

        public void CreateWorld()
        {
            AssignDates();

            CreateFormations();
            CreatePlayers();
            CreateLeagues();
            CreateTeams();
        }

        private void AssignDates()
        {
            World.WorldState.CurrentDate = new DateTime(STARTYEAR, 7, 1);
            World.WorldState.PreSeasonDate = World.WorldState.CurrentDate;
            World.WorldState.MainSeasonDate = World.WorldState.PreSeasonDate; //.AddMonths(1);
        }

        private void CreateTeams()
        {
            

            string[] TeamFirstName = TextResources.TeamList(TeamListResource.Place, ResourceLanguage.en);
            string[] TeamLastName = TextResources.TeamList(TeamListResource.Suffix, ResourceLanguage.en);

            List<string> AvailableNames = new List<string>();

            Maths rand = new Maths();
            LeagueAdapter la = new LeagueAdapter();
            List<League> Leagues = la.GetLeagues();

            foreach(League L in Leagues)
            {
                for (int i = 0; i <= MAXTEAMSPERLEAGUE - 1; i++)
                {
                    if(AvailableNames.Count == 0)
                    {
                        AvailableNames = TeamFirstName.ToList();
                    }

                    int r;

                    Team NewTeam = new Team();
                    NewTeam.SeasonStatistics = new TeamStats();

                    r = rand.RandomInclusive(0, AvailableNames.Count - 1);
                    NewTeam.Name = AvailableNames[r] + " ";
                    AvailableNames.RemoveAt(r);

                    r = rand.RandomInclusive(0, TeamLastName.GetUpperBound(0));
                    NewTeam.Name = NewTeam.Name + TeamLastName[r];
                    NewTeam.LeagueID = L.UniqueID;

                    TeamAdapter ta = new TeamAdapter();
                    int NewID = ta.AddTeam(NewTeam);

                    AssignPlayersToTeam(NewID, L.PlayerEffectivenessBase);

                    // TODO: Temporary AI manager assignment needs replacing
                    Manager NewManager = new Manager();
                    NewManager.FirstName = "Alan";
                    NewManager.LastName = NewTeam.Name.Split(Convert.ToChar(" "))[0];
                    NewManager.DateOfBirth = new DateTime(1966, 9, 26);
                    NewManager.Reputation = rand.RandomInclusive(30, 90);

                    ManagerAdapter ma = new ManagerAdapter();
                    int NewManagerID = ma.AddManager(NewManager);
                    ma.AssignToTeam(NewManagerID, NewID);
                }

            
            }
        }

        private void CreatePlayers()
        {

            string[] FirstName = TextResources.NameList(NameListResource.FirstNames, ResourceLanguage.en);
            string[] LastName = TextResources.NameList(NameListResource.LastNames, ResourceLanguage.en);
            (int min, int max)[] Ranges = { (0, 40), (20, 60), (40, 80), (60, 100)  };

            Maths rand = new Maths();

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


                int range = rand.GaussianDistributedRandom_Int(0, Ranges.GetUpperBound(0));

                NewPlayer.Agility = RandomPlayerStat(rand, Ranges[range].min, Ranges[range].max, true);
                NewPlayer.Attitude = RandomPlayerStat(rand, Ranges[range].min, Ranges[range].max, true);
                NewPlayer.Speed = RandomPlayerStat(rand, Ranges[range].min, Ranges[range].max, p != PlayerPosition.Goalkeeper);
                NewPlayer.Stamina = RandomPlayerStat(rand, Ranges[range].min, Ranges[range].max, true);

                NewPlayer.Passing = RandomPlayerStat(rand, Ranges[range].min, Ranges[range].max, p == PlayerPosition.Defender || p == PlayerPosition.Midfielder || p == PlayerPosition.Attacker);
                NewPlayer.Marking = RandomPlayerStat(rand, Ranges[range].min, Ranges[range].max, p == PlayerPosition.Defender);
                NewPlayer.Balance = RandomPlayerStat(rand, Ranges[range].min, Ranges[range].max, p == PlayerPosition.Goalkeeper);
                NewPlayer.Tackling = RandomPlayerStat(rand, Ranges[range].min, Ranges[range].max, p == PlayerPosition.Defender || p == PlayerPosition.Midfielder);
                NewPlayer.Shooting = RandomPlayerStat(rand, Ranges[range].min, Ranges[range].max, p == PlayerPosition.Attacker || p == PlayerPosition.Striker);
                NewPlayer.Handling = RandomPlayerStat(rand, Ranges[range].min, Ranges[range].max, p == PlayerPosition.Goalkeeper);
                NewPlayer.Heading = RandomPlayerStat(rand, Ranges[range].min, Ranges[range].max, p == PlayerPosition.Midfielder || p == PlayerPosition.Attacker);
                NewPlayer.Influence = RandomPlayerStat(rand, Ranges[range].min, Ranges[range].max, rand.RandomInclusive(0, 1) == 1);

                NewPlayer.Wages = rand.RandomInclusive(5, 80) * 100;

                NewPlayer.DateOfBirth = DateTime.Now.AddDays(rand.RandomInclusive(0 - (32 * 365), 0-(18*365)));

                PlayerAdapter pa = new PlayerAdapter();
                pa.AddPlayer(NewPlayer);
            }
        }

        private int RandomPlayerStat(Maths m, int min, int max, bool preferThisPosition)
        {
            if (!preferThisPosition)
            {
                min -= 20;
                if (min < 1)
                    min = 1;
            }
            return m.GaussianDistributedRandom_Int(min, max);
        }

        private void CreateLeagues()
        {
            LeagueAdapter la = new LeagueAdapter();

            League L;

            L = new League();
            L.Name = "Premiership";
            L.ShortName = "VP";
            L.PlayerEffectivenessBase = 85;
            la.AddLeague(L);

            L = new League();
            L.Name = "League 1";
            L.ShortName = "L1";
            L.PlayerEffectivenessBase = 65;
            la.AddLeague(L);

            L = new League();
            L.Name = "League 2";
            L.ShortName = "L2";
            L.PlayerEffectivenessBase = 40;
            la.AddLeague(L);

            L = new League();
            L.Name = "Conference";
            L.ShortName = "CON";
            L.PlayerEffectivenessBase = 30;
            la.AddLeague(L);
        }

        private void AssignPlayersToTeam(int TeamID, int effBase)
        {
            const int PlayerEffectiveBand = 15;
            const int PlayerEffectiveBandDown = -2;
            Maths rand = new Maths();

            int max = effBase + rand.RandomInclusive(PlayerEffectiveBandDown, PlayerEffectiveBand);
            int min = max - PlayerEffectiveBand;

            AssignPlayersToTeam_Worker(TeamID, PlayerPosition.Goalkeeper, PlayerPositionSide.Centre, rand.RandomInclusive(1, 3), min, max);

            AssignPlayersToTeam_Worker(TeamID, PlayerPosition.Defender, PlayerPositionSide.Left, rand.RandomInclusive(1, 3), min, max);
            AssignPlayersToTeam_Worker(TeamID, PlayerPosition.Defender, PlayerPositionSide.Centre, rand.RandomInclusive(3, 5), min, max);
            AssignPlayersToTeam_Worker(TeamID, PlayerPosition.Defender, PlayerPositionSide.Right, rand.RandomInclusive(1, 3), min, max);

            AssignPlayersToTeam_Worker(TeamID, PlayerPosition.Midfielder, PlayerPositionSide.Left, rand.RandomInclusive(1, 3), min, max);
            AssignPlayersToTeam_Worker(TeamID, PlayerPosition.Midfielder, PlayerPositionSide.Centre, rand.RandomInclusive(3, 5), min, max);
            AssignPlayersToTeam_Worker(TeamID, PlayerPosition.Midfielder, PlayerPositionSide.Right, rand.RandomInclusive(1, 3), min, max);

            AssignPlayersToTeam_Worker(TeamID, PlayerPosition.Attacker, PlayerPositionSide.Left, rand.RandomInclusive(0, 1), min, max);
            AssignPlayersToTeam_Worker(TeamID, PlayerPosition.Attacker, PlayerPositionSide.Centre, rand.RandomInclusive(1, 2), min, max);
            AssignPlayersToTeam_Worker(TeamID, PlayerPosition.Attacker, PlayerPositionSide.Right, rand.RandomInclusive(0, 1), min, max);

            AssignPlayersToTeam_Worker(TeamID, PlayerPosition.Striker, PlayerPositionSide.Centre, rand.RandomInclusive(0, 2), min, max);

            PlayerAdapter pa = new PlayerAdapter();
            pa.SetUseInitialOnPlayers(TeamID); // Run here instead of in PlayerAdapter.AssignToTeam() so it's only run once per team
        }

        private void AssignPlayersToTeam_Worker(int TeamID, PlayerPosition pos, PlayerPositionSide side, int max, int minEffectiveness, int maxEffectiveness)
        {
            Maths rand = new Maths();

            for (int i = 1; i <= max; i++)
            {
                // Select free players of given type
                PlayerAdapter pa = new PlayerAdapter();
                List<Player> playerList = pa.GetPlayers(-1, pos, side, minEffectiveness, maxEffectiveness);

                int found = playerList.Count();
                if (found == 0)
                {
                    playerList = (from p in pa.GetPlayers(-1, pos, side)
                                  where p.OverallRating < maxEffectiveness
                                  select p).ToList();

                    found = playerList.Count();

                    if (found == 0)
                        throw new Exception("Not enough players");
                }

                int which = rand.GaussianDistributedRandom_Int(0, found - 1);
                PlayerAdapter pad = new PlayerAdapter();

                pad.AssignToTeam(playerList[which].UniqueID, TeamID, true);
            }
        }

        private void CreateFormations()
        {
            List<Point2> p;
            FormationAdapter fa = new FormationAdapter();

            // 442 ----------------------------
            p = new List<Point2>();

            p.Add(new Point2(2, 0)); // GK

            p.Add(new Point2(0, 1)); // Def
            p.Add(new Point2(1, 1));
            p.Add(new Point2(3, 1));
            p.Add(new Point2(4, 1));

            p.Add(new Point2(0, 3)); // Mid
            p.Add(new Point2(1, 3));
            p.Add(new Point2(3, 3));
            p.Add(new Point2(4, 3));

            p.Add(new Point2(1, 6)); // Att/Stk
            p.Add(new Point2(3, 6));

            fa.AddFormation("442", p, true);
            p = null;
            // --------------------------------


            // 532 ----------------------------
            p = new List<Point2>();

            p.Add(new Point2(2, 0)); // GK

            p.Add(new Point2(0, 2)); // Def
            p.Add(new Point2(1, 2));
            p.Add(new Point2(2, 2));
            p.Add(new Point2(3, 2));
            p.Add(new Point2(4, 2));

            p.Add(new Point2(0, 4)); // Mid
            p.Add(new Point2(2, 4));
            p.Add(new Point2(4, 4));

            p.Add(new Point2(1, 6)); // Att/Stk
            p.Add(new Point2(3, 6));

            fa.AddFormation("532", p, true);
            p = null;
            // --------------------------------


            // Diamond ------------------------
            p = new List<Point2>();

            p.Add(new Point2(2, 0)); // GK

            p.Add(new Point2(0, 2)); // Def
            p.Add(new Point2(1, 2));
            p.Add(new Point2(3, 2));
            p.Add(new Point2(4, 2));

            p.Add(new Point2(2, 3)); // Mid
            p.Add(new Point2(1, 4));
            p.Add(new Point2(3, 4));

            p.Add(new Point2(2, 5)); // Att/Stk
            p.Add(new Point2(1, 6));
            p.Add(new Point2(3, 6));

            fa.AddFormation("Diamond", p, true);
            p = null;
            // --------------------------------
        }

    }
}
