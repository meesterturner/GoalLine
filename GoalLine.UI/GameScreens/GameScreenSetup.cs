using GoalLine.Structures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoalLine.UI.GameScreens
{
    public class GameScreenSetup
    {
        public List<String> MainButtons { get; set; } = new List<String>();
        public bool ShowContinueButton { get; set; }
        public bool ShowDate { get; set; } = true;
        public string Title1 { get; set; }
        public string Title2 { get; set; }
        public GameWindow Parent { get; set; }

        public Team TeamData;
        public Player PlayerData;
        public Manager ManagerData;

    }
}
