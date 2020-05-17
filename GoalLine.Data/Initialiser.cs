﻿using System;
using System.Collections.Generic;
using System.IO;
using GoalLine.Structures;
using System.Linq;
using System.Security.Cryptography.X509Certificates;

namespace GoalLine.Data
{
    public class Initialiser
    {
        private const int STARTYEAR = 2020;
        
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
            const int MaxTeams = 16;

            string[] TeamFirstName = new string[] {"Grimsby","Lincoln","Scunthorpe","Hull", "Manchester", "Norwich", "Reading","Accrington",
                                                   "Sheffield", "Leeds","Nottingham","Croydon","Maidstone", "Peterborough", "Swindon", "Southampton",
                                                   "Cardiff", "Brighton", "Leicester", "Oxford", "Newcastle", "York", "Birmingham"};

            string[] TeamLastName = new string[] { "Town", "City", "United", "Borough", "County", "Rovers" };

            Dictionary<string, bool> UsedNames = new Dictionary<string, bool>();

            Random rand = new Random();
            for (int i = 0; i <= MaxTeams-1; i++)
            {
                int r;

                Team NewTeam = new Team();
                NewTeam.UniqueID = i;

                while (true)
                {
                    r = rand.Next(0, TeamFirstName.GetUpperBound(0));
                    NewTeam.Name = TeamFirstName[r] + " ";

                    r = rand.Next(0, TeamLastName.GetUpperBound(0));
                    NewTeam.Name = NewTeam.Name + TeamLastName[r];

                    if (!UsedNames.ContainsKey(NewTeam.Name))
                    {
                        break;
                    }
                   
                }

                UsedNames.Add(NewTeam.Name, true);

                NewTeam.ManagerID = -1;
                NewTeam.LeagueID = 0;

                World.Teams.Add(NewTeam);

                

                AssignPlayersToTeam(i);
            }
        }

        private void CreatePlayers()
        {
            const int MaxPlayers = 5000;

            // TODO: This will be replaced with something better later.
            string[] FirstName = new string[] {"Paul","Chris","Andrew","Daniel", "Alex", "Nick","Lee","Luke","David","Duncan","Jack","Michael",
                                               "Joseph","Richard","John","Mark","Stuart","Gordon","Alan","Graham","Connor", "Martin", "Matt",
                                               "Steve", "Thomas", "Tony", "Wayne", "Phil", "Jason", "Lewis", "James", "Scott", "Bruce", "Gary", 
                                               "Glen", "Vince", "Barry", "Bailey", "Ashley", "Lucas", "Shaun", "Dennis", "Kenny", "Eric", "Aaron",
                                               "Ewan", "Malcolm", "Adam", "Harry", "Harvey", "Sam", "Tim", "Neil" };

            string[] LastName = new string[] { "Turner","Gibson","Smith","Watson","Edwards","Johnson","Barley","Swinburn","Parker",
                                               "Reynolds","Hewson","Ibbotson","Read","Bradley","Shaw","Jones", "Palmer", "Burton", "Riley",
                                               "Redford","Bell","Ross","Blackburn","Beckett","Gray", "Hill", "Ferguson","Charlton","Thompson",
                                               "Wilkinson","Playford","Bevan", "Taylor", "Brown", "Armitage", "Hewins","Everitt", "Forrester",
                                               "Walker", "Lee", "Wilson","Goddard", "Boardman", "Wells", "Spencer", "Hayes", "Jackson", "Lovell",
                                               "Lawson", "Cole", "Manford", "Curtis", "Lewis", "Lee","Severn", "Miller", "Carter","Goodwin", 
                                               "Ward", "Dugard", "Price", "Ridley", "James", "King", "Scott", "Clark", "Lamb", "Leaning", "Wayne",
                                               "Sharp", "Law", "Hardy", "Campbell", "Swales", "Grayson", "Sandiford", "Downing", "Blunt", "Waters",
                                               "Newmarch", "Thomas", "Speed", "Henshaw", "Thurley", "Rusling", "Byron", "Schofield", "Davidson",
                                               "Steel", "North", "West", "Barney", "Page", "Mitchell", "Harvey", "Dixon", "Cox", "Chase", "Hunter",
                                               "Newton", "Simpson", "Radley", "Vessey", "Russell", "Kirkham", "O'Brien", "O'Sullivan", "Harris",
                                               "Banks", "Ford", "Fisher", "Lane", "Messenger", "Douglas", "Moore", "Wright", "Chamberlain",
                                               "Wood", "Rees", "Young", "Butler", "Jenkinson", "Ramsey", "Oldroyd", "Barker", "Hammond", "Richardson"};


            Random rand = new Random();
            Random posRand = new Random();

            for (int i = 0; i <= MaxPlayers - 1; i++)
            {
                int r;

                Player NewPlayer = new Player();
                NewPlayer.UniqueID = i;

                r = rand.Next(0, FirstName.GetUpperBound(0));
                NewPlayer.FirstName = FirstName[r];

                r = rand.Next(0, 100);
                if(r < 7)
                {
                    // Double-barrelled surname
                    r = rand.Next(0, LastName.GetUpperBound(0));
                    NewPlayer.LastName = LastName[r] + "-";

                    int r2 = r; // avoiding any Smith-Smith names
                    while(r2 == r)
                    {
                        r2 = rand.Next(0, LastName.GetUpperBound(0));
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
                        r = rand.Next(0, LastName.GetUpperBound(0));
                        NewPlayer.LastName = LastName[r];
                    }
                }

                r = posRand.Next(10, 59);
                r = r / (int)10;

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
                    NewPlayer.PreferenceLeft = rand.Next(0,100);
                    NewPlayer.PreferenceRight = rand.Next(0, 100);
                    NewPlayer.PreferenceMiddle = rand.Next(0, 100);

                }

                NewPlayer.Agility = rand.Next(0, 100);
                NewPlayer.Attitude = rand.Next(0, 100);
                NewPlayer.Speed = rand.Next(0, 100);
                NewPlayer.Stamina = rand.Next(0, 100);

                NewPlayer.Wages = rand.Next(5, 80) * 100;

                NewPlayer.DateOfBirth = DateTime.Now.AddDays(rand.Next(0 - (32 * 365), 0-(18*365)));
                NewPlayer.CurrentTeam = -1;

                World.Players.Add(NewPlayer);
            }
        }

        private void CreateLeagues()
        {
            // TODO: A few more than this, lol!
            League L = new League();
            L.UniqueID = 0;
            L.Name = "Fenwick League";

            World.Leagues.Add(L);
        }
        private void AssignPlayersToTeam(int TeamID)
        {
            Random rand = new Random();
            AssignPlayersToTeam_Worker(TeamID, PlayerPosition.Goalkeeper, PlayerPositionSide.Centre, rand.Next(1, 3));

            AssignPlayersToTeam_Worker(TeamID, PlayerPosition.Defender, PlayerPositionSide.Left, rand.Next(1, 3));
            AssignPlayersToTeam_Worker(TeamID, PlayerPosition.Defender, PlayerPositionSide.Centre, rand.Next(3, 5));
            AssignPlayersToTeam_Worker(TeamID, PlayerPosition.Defender, PlayerPositionSide.Right, rand.Next(1, 3));

            AssignPlayersToTeam_Worker(TeamID, PlayerPosition.Midfielder, PlayerPositionSide.Left, rand.Next(1, 3));
            AssignPlayersToTeam_Worker(TeamID, PlayerPosition.Midfielder, PlayerPositionSide.Centre, rand.Next(3, 5));
            AssignPlayersToTeam_Worker(TeamID, PlayerPosition.Midfielder, PlayerPositionSide.Right, rand.Next(1, 3));

            AssignPlayersToTeam_Worker(TeamID, PlayerPosition.Attacker, PlayerPositionSide.Left, rand.Next(0, 1));
            AssignPlayersToTeam_Worker(TeamID, PlayerPosition.Attacker, PlayerPositionSide.Centre, rand.Next(1, 2));
            AssignPlayersToTeam_Worker(TeamID, PlayerPosition.Attacker, PlayerPositionSide.Right, rand.Next(0, 1));

            AssignPlayersToTeam_Worker(TeamID, PlayerPosition.Striker, PlayerPositionSide.Centre, rand.Next(0, 2));
        }

        private void AssignPlayersToTeam_Worker(int TeamID, PlayerPosition pos, PlayerPositionSide side, int max)
        {
            Random rand = new Random();

            for (int i = 1; i <= max; i++)
            {
                // Select free players of given type
                IEnumerable<Player> playerQuery =
                    from player in World.Players
                    where player.CurrentTeam == -1 &&
                          player.Position == pos &&
                          player.PreferredSide == side
                    select player;

                List<Player> playerList = playerQuery.ToList();

                int found = playerList.Count();
                if (found == 0)
                {
                    throw new Exception("Not enough players");
                }

                int which = rand.Next(0, found - 1);
                PlayerAdapter pad = new PlayerAdapter();

                pad.AssignToTeam(playerList[which].UniqueID, TeamID);
            }
        }

    }
}
