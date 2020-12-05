using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoalLine.UI.GameScreens
{
    public enum ScreenReturnCode 
    {
        None = 0,
        Ok = 1,
        Cancel = 2,
        Next = 3,
        MatchdayComplete = 100,
        Error = 255
    }
}
