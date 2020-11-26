using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using GoalLine.Matchday;
using GoalLine.Structures;
using GoalLine.Data;
using GoalLine.UI.Utils;

namespace GoalLine.UI.GameScreens
{
    /// <summary>
    /// Interaction logic for MatchdayMain.xaml
    /// </summary>
    public partial class MatchdayMain : UserControl, IGameScreen
    {
        public GameScreenSetup SetupData { get; set; }

        public MatchdayMain()
        {
            InitializeComponent();
        }

        public ScreenReturnData MainButtonClick(int buttonId)
        {
            RunMatchday();

            return null;
        }

        public void SetupGameScreenData(GameScreenSetup dataFromUI)
        {
            SetupData = dataFromUI;
            SetupData.MainButtons.Add("Start");

            SetupData.ShowContinueButton = false;
            SetupData.ShowDate = false;

            SetupData.Title1 = "Match Day";
            SetupData.Title2 = "Team One v Team Two";
        }

        private void RunMatchday()
        {
            MatchDayRunner dr = new MatchDayRunner();
            dr.Run(new MatchdayCallback(this));
        }
    }
}
