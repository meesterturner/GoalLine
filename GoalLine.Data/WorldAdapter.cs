using System;
using System.IO;

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

        public string SaveGameName
        {
            get
            {
                return World.WorldState.SaveGameName;
            }
            set
            {
                World.WorldState.SaveGameName = value.Replace(Path.DirectorySeparatorChar.ToString(), "")
                                                  .Replace(Path.VolumeSeparatorChar.ToString(), "")
                                                  .Replace(Path.AltDirectorySeparatorChar.ToString(), "")
                                                  .Trim(); ;
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

        public int GetNextEmailID()
        {
            World.WorldState.NextEmailID++;
            return World.WorldState.NextEmailID;
        }
    }
}
