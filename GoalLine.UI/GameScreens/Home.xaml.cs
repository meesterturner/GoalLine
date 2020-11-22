using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using GoalLine.Data;
using GoalLine.Structures;
using GoalLine.UI.GameScreens;
using GoalLine.UI.Controls;

namespace GoalLine.UI.GameScreens
{
    /// <summary>
    /// Interaction logic for Home.xaml
    /// </summary>
    public partial class Home : UserControl, IGameScreen
    {

        public GameScreenSetup SetupData { get; set; }

        public Home()
        {
            InitializeComponent();
        }

        public void SetupGameScreenData(GameScreenSetup dataFromUI)
        {
            if(dataFromUI.ManagerData == null)
            {
                throw new Exception("ManagerData is null");
            }

            TeamAdapter ta = new TeamAdapter();

            SetupData = dataFromUI;
            SetupData.TeamData = ta.GetTeamByManager(SetupData.ManagerData.UniqueID);
            SetupData.ShowContinueButton = true;

            SetupData.Title1 = SetupData.TeamData.Name;
            SetupData.Title2 = SetupData.ManagerData.Name;

            UpdateLeagueTable();
        }

        private void UpdateLeagueTable()
        {
            LeagueAdapter la = new LeagueAdapter();
            lstLeague.Title = la.GetLeague(SetupData.TeamData.LeagueID).Name;

            List<ListColumn> c = new List<ListColumn>();
            c.Add(new ListColumn("Pos", 75));
            c.Add(new ListColumn("Team", 300));
            c.Add(new ListColumn("P", 75, HorizontalAlignment.Right));
            c.Add(new ListColumn("W", 75, HorizontalAlignment.Right));
            c.Add(new ListColumn("D", 75, HorizontalAlignment.Right));
            c.Add(new ListColumn("L", 75, HorizontalAlignment.Right));
            c.Add(new ListColumn("F", 75, HorizontalAlignment.Right));
            c.Add(new ListColumn("A", 75, HorizontalAlignment.Right));
            c.Add(new ListColumn("GD", 75, HorizontalAlignment.Right));
            c.Add(new ListColumn("Pts", 75, HorizontalAlignment.Right));

            lstLeague.Columns = c;

            
            List<LeagueTableRecord> table = la.LeagueTable(SetupData.TeamData.LeagueID);

            List<ListRow> rows = new List<ListRow>();

            for(int i = 0; i < table.Count(); i++)
            {
                LeagueTableRecord t = table[i];
                List<string> rowData = new List<string>();
                rowData.Add(i.ToString());
                rowData.Add(t.Name);
                rowData.Add(t.SeasonStatistics.GamesPlayed.ToString());
                rowData.Add(t.SeasonStatistics.Won.ToString());
                rowData.Add(t.SeasonStatistics.Drawn.ToString());
                rowData.Add(t.SeasonStatistics.Lost.ToString());
                rowData.Add(t.SeasonStatistics.GoalsScored.ToString());
                rowData.Add(t.SeasonStatistics.GoalsConceded.ToString());
                rowData.Add(t.SeasonStatistics.GoalDifference.ToString());
                rowData.Add(t.SeasonStatistics.Points.ToString());

                rows.Add(new ListRow(i, rowData));
            }

            lstLeague.Rows = rows;
            lstLeague.Render();
        }

        public ScreenReturnData MainButtonClick(int buttonId)
        {
            throw new NotImplementedException();

            //return new ScreenReturnData(ScreenReturnCode.Ok);
        }
    }
}
