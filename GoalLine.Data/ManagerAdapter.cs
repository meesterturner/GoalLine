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
        public int AddManager(Manager m)
        {
            int NextID = World.Managers.Count;
            m.UniqueID = NextID;
            World.Managers.Add(m);

            return NextID;
        }

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

        public List<Manager> GetHumanManagers()
        {
            return (from m in World.Managers
                    where m.Human == true
                    select m).ToList();
        }

        public Manager GetManager(int ManagerID)
        {
            return World.Managers[ManagerID];
        }
    }
}
