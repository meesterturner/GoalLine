using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GoalLine.Structures;
using GoalLine.Data;

namespace GoalLine.Processes.ProcessLogic
{
    class DirectorLogic
    {
        public void GoodAndBadFixtureFeedback(List<Fixture> fixtures)
        {
            ManagerAdapter ma = new ManagerAdapter();
            EmailAdapter ea = new EmailAdapter();

            foreach (Manager m in ma.GetHumanManagers())
            {
                foreach (Fixture f in fixtures)
                {
                    for (int team = 0; team <= 1; team++)
                    {
                        if(f.TeamIDs[team] == m.CurrentTeam) // Find manager's team
                        {
                            int opp = 1 - team; // Opposition team

                            if(Math.Abs(f.Score[team] - f.Score[opp]) >= 3)
                            {
                                if (f.Score[team] > f.Score[opp])
                                    ea.SendEmail(m.UniqueID, EmailType.GoodMatch, new List<int>() { f.Score[0], f.Score[1], f.TeamIDs[opp] });

                                if (f.Score[team] < f.Score[opp])
                                    ea.SendEmail(m.UniqueID, EmailType.BadMatch, new List<int>() { f.Score[0], f.Score[1], f.TeamIDs[opp] });

                            }
                        }
                    }
                }
            }

            
        }
    }
}
