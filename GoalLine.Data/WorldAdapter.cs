using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoalLine.Data
{
    public class WorldAdapter
    {
        public DateTime CurrentDate
        {
            get
            {
                return World.WorldState.CurrentDate;
            }
        }

        public DateTime MainSeasonDate 
        { 
            get 
            {
                return World.WorldState.MainSeasonDate;
            }
            set
            {
                World.WorldState.MainSeasonDate = value;
            }
        }

        public DateTime PreSeasonDate
        {
            get
            {
                return World.WorldState.PreSeasonDate;
            }
            set
            {
                World.WorldState.PreSeasonDate = value;
            }
        }

        public int CurrentManagerID
        {
            get
            {
                return World.WorldState.CurrentManagerID;
            }
            set
            {
                World.WorldState.CurrentManagerID = value;
            }
        }

        public DateTime AdvanceDate()
        {
            return AdvanceDate(1);
        }

        public DateTime AdvanceDate(int Days)
        {
            World.WorldState.CurrentDate = World.WorldState.CurrentDate.AddDays(Days);
            return World.WorldState.CurrentDate;
        }
    }
}
