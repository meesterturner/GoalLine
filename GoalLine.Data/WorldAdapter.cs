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
                return World.CurrentDate;
            }
        }

        public DateTime MainSeasonDate 
        { 
            get 
            {
                return World.MainSeasonDate;
            }
            set
            {
                World.MainSeasonDate = value;
            }
        }

        public DateTime PreSeasonDate
        {
            get
            {
                return World.PreSeasonDate;
            }
            set
            {
                World.PreSeasonDate = value;
            }
        }

        public int CurrentManagerID
        {
            get
            {
                return World.CurrentManagerID;
            }
            set
            {
                World.CurrentManagerID = value;
            }
        }

        public DateTime AdvanceDate()
        {
            return AdvanceDate(1);
        }

        public DateTime AdvanceDate(int Days)
        {
            World.CurrentDate = World.CurrentDate.AddDays(Days);
            return World.CurrentDate;
        }
    }
}
