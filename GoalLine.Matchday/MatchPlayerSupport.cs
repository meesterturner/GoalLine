using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GoalLine.Data;

namespace GoalLine.Matchday
{
    class MatchPlayerSupport
    {
        Maths maths = new Maths();

        public MatchEventType RandomiseEvent(List<MatchEventChanceItem> events)
        {
            double totChance = 0;
            List<MatchEventChanceItem> aggregated = new List<MatchEventChanceItem>();
            int count = events.Count();

            for(int i = 0; i < count; i++)
            {
                totChance += events[i].Chance;
                aggregated.Add(new MatchEventChanceItem(events[i].evType, totChance));
            }

            if(totChance < 1.0f)
            {
                count++;
                totChance = 1.0;
                aggregated.Add(new MatchEventChanceItem(MatchEventType.None, totChance));
            }

            double rand = maths.GaussianDistributedRandom(0, totChance);

            for (int i = 0; i < count; i++)
            {
                if(aggregated[i].Chance <= rand)
                {
                    return aggregated[i].evType;
                }
            }

            throw new ArithmeticException();
        }
    }
}
