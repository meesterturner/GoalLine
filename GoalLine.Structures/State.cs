using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoalLine.Structures
{
    public class State
    {
        public DateTime CurrentDate { get; set; }
        public DateTime PreSeasonDate { get; set; }
        public DateTime MainSeasonDate { get; set; }
        public int CurrentManagerID { get; set; }
        public string SaveGameName { get; set; } = "";
    }
}