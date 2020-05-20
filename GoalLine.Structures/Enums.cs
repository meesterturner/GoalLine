using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoalLine.Structures
{
    public enum PlayerPosition
    {
        None = 0,
        Goalkeeper = 1,
        Defender = 2,
        Midfielder = 3,
        Attacker = 4,
        Striker = 5
    }

    public enum PlayerPositionSide
    {
        Left = -10,
        Centre = 0,
        Right = 10
    }

    public enum PlayerSelectionStatus
    {
        None = 0,
        Starting = 1,
        Sub = 2
    }
}
