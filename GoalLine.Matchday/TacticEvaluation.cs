using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GoalLine.Structures;

namespace GoalLine.Matchday
{
    public class TacticEvaluation
    {
        private int[] PositionCount = new int[6];
        private int[] PositionTotal = new int[6];

        public int Goalkeeping { get => Average(PlayerPosition.Goalkeeper); }
        public int Defence { get => Average(PlayerPosition.Defender); }
        public int Midfield { get => Average(PlayerPosition.Midfielder); }
        public int Attack { get => Average(PlayerPosition.Attacker); }
        public int Striker { get => Average(PlayerPosition.Striker); }

        private int Average(PlayerPosition pos)
        {
            int p = (int)pos;
            if(PositionCount[p] == 0)
            {
                return 0;
            }

            return PositionTotal[p] / PositionCount[p];
        }

        public void AddRatingForPosition(PlayerPosition p, int PlayerRating)
        {
            PositionCount[(int)p]++;
            PositionTotal[(int)p] += PlayerRating;
        }
    }
}