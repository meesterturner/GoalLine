using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoalLine.Structures
{
    public class Team
    {
        public int UniqueID { get; set; }
        public string Name { get; set; }
        public Dictionary<int, TeamPlayer> Players { get; set; } = new Dictionary<int, TeamPlayer>();
        public Dictionary<int, TeamPlayer> LastKnownPick { get; set; } = new Dictionary<int, TeamPlayer>();
        public int ManagerID { get; set; }
        public int LeagueID { get; set; }
        public TeamStats SeasonStatistics { get; set; }

    }

    public class TeamPlayer
    {
        public int PlayerID { get; set; }
        public PlayerSelectionStatus Selected { get; set; }

        public TeamPlayer()
        {

        }

        public TeamPlayer(int PlayerID)
        {
            this.PlayerID = PlayerID;
            this.Selected = PlayerSelectionStatus.None;
        }
    }

    public class TeamStats
    {
        public int Points { get; set; }
        public int Won { get; set; }
        public int Lost { get; set; }
        public int Drawn { get; set; }
        public int GamesPlayed { get; set; }
        public int GoalsScored { get; set; }
        public int GoalsConceded { get; set; }
        public int GoalDifference
        {
            get
            {
                return GoalsScored - GoalsConceded;
            }
        }
    }
}
