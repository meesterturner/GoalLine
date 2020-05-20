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
        public int ManagerID { get; set; }
        public int LeagueID { get; set; }

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
}
