using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoalLine.Structures
{
    public class Manager : Person
    {
        const int DefaultReputation = 50;

        public Manager()
        {
            Reputation = DefaultReputation;
        }

        public int Reputation { get; set; }
        public bool Human { get; set; }
    }
}
