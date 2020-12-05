using System.Threading;
using System.Windows.Controls;
using GoalLine.Matchday;
using GoalLine.UI.Utils;
using GoalLine.Resources;
using GoalLine.Structures;
using GoalLine.Data;

namespace GoalLine.UI.GameScreens
{
    /// <summary>
    /// Interaction logic for MatchdayMain.xaml
    /// </summary>
    public partial class MatchdayMain : UserControl, IGameScreen
    {
        public GameScreenSetup SetupData { get; set; }
        public bool MatchdayComplete { get; set; }

        public MatchdayMain()
        {
            InitializeComponent();
            pitPitch.CentreBall();
        }

        public ScreenReturnData MainButtonClick(int buttonId)
        {
            if(!MatchdayComplete)
            {
                Thread rm = new Thread(this.RunMatchday);
                rm.IsBackground = true;
                rm.Start();

                return new ScreenReturnData(ScreenReturnCode.None);
            } else
            {
                FixtureAdapter fa = new FixtureAdapter();
                WorldAdapter wa = new WorldAdapter();
                
                SetupData.Parent.ShowResultsScreen(fa.GetFixtures(wa.CurrentDate));
                return new ScreenReturnData(ScreenReturnCode.None);
            }
            
        }

        public void SetupGameScreenData(GameScreenSetup dataFromUI)
        {
            SetupData = dataFromUI;
            SetupData.MainButtons.Add(LangResources.CurLang.Start);

            SetupData.ShowContinueButton = false;
            SetupData.ShowDate = true;

            SetupData.Title1 = LangResources.CurLang.MatchDay;
            SetupData.Title2 = "Team One v Team Two";
        }

        private void RunMatchday()
        {
            MatchDayRunner dr = new MatchDayRunner();
            dr.Run(new MatchdayCallback(this));
        }

        public ScreenReturnData ContinueButtonClick()
        {
            throw new System.NotImplementedException();
        }
    }
}
