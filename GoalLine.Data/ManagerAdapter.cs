using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GoalLine.Structures;
namespace GoalLine.Data
{
    public class ManagerAdapter
    {
        public void AssignToTeam(int ManagerID, int TeamID)
        {
            Manager m = World.Managers[ManagerID];
            int OldTeam = m.CurrentTeam;

            if (OldTeam > -1)
            {
                World.Teams[OldTeam].ManagerID = -1;
            }

            World.Managers[ManagerID].CurrentTeam = TeamID;
            World.Teams[TeamID].ManagerID = ManagerID;
        }
    }
}
