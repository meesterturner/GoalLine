using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using GoalLine.Structures;

namespace GoalLine.Data
{
    public static class World
    {
        public static DateTime CurrentDate { get; set; }
        public static DateTime PreSeasonDate { get; set; }
        public static DateTime MainSeasonDate { get; set; }

        public static int CurrentManagerID { get; set; }

        public static List<Player> Players { get; set; } = new List<Player>();
        public static List<Team> Teams { get; set; } = new List<Team>();
        public static List<Manager> Managers { get; set; } = new List<Manager>();
        public static List<League> Leagues { get; set; } = new List<League>();

        public static List<Fixture> Fixtures { get; set; } = new List<Fixture>();
    }
}
