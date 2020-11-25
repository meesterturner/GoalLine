using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using GoalLine.Structures;

namespace GoalLine.Data
{
    static class World
    {
        // REMEMBER! When adding classes here, update the GameIO.LoadGame() and GameIO.SaveGame() methods

        public static State WorldState = new State();  

        public static List<Player> Players { get; set; } = new List<Player>();
        public static List<Team> Teams { get; set; } = new List<Team>();
        public static List<Manager> Managers { get; set; } = new List<Manager>();
        public static List<League> Leagues { get; set; } = new List<League>();

        public static List<Fixture> Fixtures { get; set; } = new List<Fixture>();
        public static List<Formation> Formations { get; set; } = new List<Formation>(); // TODO: Load and save later (maybe)
    }
}
