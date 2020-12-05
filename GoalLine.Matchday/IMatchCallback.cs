﻿using GoalLine.Structures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoalLine.Matchday
{
    public interface IMatchCallback
    {
        string Commentary { get; set; }
        MatchStatus MatchStatus { get; set; }
        MatchEventType EventType { get; set; }
        int PauseTime { get; set; }

        void UpdateUI();
        void MatchStarting(Fixture f, bool Interactive);
        void MatchFinished(Fixture f, bool Interactive);
        void MatchdayComplete();
    }
}
