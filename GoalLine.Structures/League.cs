﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoalLine.Structures
{
    public class League
    {
        public int UniqueID { get; set; }
        public string Name { get; set; }
    }

    public class LeagueTableRecord : TeamStats
    {
        public int TeamID { get; set; }
        public string Name { get; set; }
    }
}
