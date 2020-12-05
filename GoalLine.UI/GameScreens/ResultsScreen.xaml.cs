using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using GoalLine.Resources;
using GoalLine.Data;
using GoalLine.Structures;
using GoalLine.UI.Controls;

namespace GoalLine.UI.GameScreens
{
    /// <summary>
    /// Interaction logic for ResultsScreen.xaml
    /// </summary>
    public partial class ResultsScreen : UserControl, IGameScreen
    {
        public ResultsScreen()
        {
            InitializeComponent();
        }

        public GameScreenSetup SetupData { get; set; }

        public ScreenReturnData ContinueButtonClick()
        { 
            return new ScreenReturnData(ScreenReturnCode.MatchdayComplete);
        }

        public ScreenReturnData MainButtonClick(int buttonId)
        {
            throw new NotImplementedException();
        }

        public void SetupGameScreenData(GameScreenSetup dataFromUI)
        {

            SetupData = dataFromUI;
            SetupData.ShowContinueButton = true;
            SetupData.ShowDate = true;

            SetupData.Title1 = "Results";
            SetupData.Title2 = "";

            ShowResults();
        }

        private void ShowResults()
        {
            lstResults.Title = "Results";
            lstResults.Columns = new List<ListColumn>()
            {
                new ListColumn(LangResources.CurLang.League, 175),
                new ListColumn(LangResources.CurLang.Home, 250),
                new ListColumn(" ", 50, HorizontalAlignment.Center),
                new ListColumn(LangResources.CurLang.Away, 250, HorizontalAlignment.Right)
            };

            List<ListRow> rows = new List<ListRow>();
            TeamAdapter ta = new TeamAdapter();
            LeagueAdapter la = new LeagueAdapter();

            foreach (Fixture f in SetupData.FixtureData)
            {
                rows.Add(new ListRow(f.UniqueID, new List<object>() {
                    la.GetLeague(f.LeagueID).Name,
                    ta.GetTeam(f.TeamIDs[0]).Name,
                    string.Format("{0} - {1}", f.Score[0].ToString(), f.Score[1].ToString()),
                    ta.GetTeam(f.TeamIDs[1]).Name
                }));
            }

            lstResults.Rows = rows;
            lstResults.SelectionMode = SelectMode.Highlight;
        }
    }
}
