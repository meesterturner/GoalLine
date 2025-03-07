﻿using System;
using System.Collections.Generic;
using GoalLine.Structures;

namespace GoalLine.Matchday
{
    public class MatchStatus
    {
        public MatchSegment Segment { get; set; }
        public int SegmentTimeSeconds { get; set; }
        public int MatchTimeSeconds { get; set; }
        public int TotalTime { get; set; }
        public int TotalTimeUnits { get; set; }
        public int[] Score { get; set; } = new int[2];
        public int PossessionTeam { get; set; }
        public int[] PossessionUnits { get; set; } = new int[2];
        public int[] Shots { get; set; } = new int[2];
        public List<PlayerStatus>[] PlayerStatuses { get; set; } = new List<PlayerStatus>[2];
        public TacticEvaluation[] Evaluation { get; set; } = new TacticEvaluation[2];
        public int[] OverallPlayerEffectiveRating { get; set; } = new int[2];
        public double BallX { get; set; }
        public double BallY { get; set; }

        public int PossessionPercentage(int Team)
        {
            if(TotalTimeUnits == 0)
            {
                return 50;
            } else
            {
                double perc = (Convert.ToDouble(PossessionUnits[Team]) / Convert.ToDouble(TotalTimeUnits)) * 100;
                return (int)perc;
            }
        }
    }
}
