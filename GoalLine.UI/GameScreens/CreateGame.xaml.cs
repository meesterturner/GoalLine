using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using GoalLine.Data;
using GoalLine.Structures;
using GoalLine.UI.GameScreens;
using GoalLine.UI.Controls;

namespace GoalLine.UI
{
    /// <summary>
    /// Interaction logic for StartControl.xaml
    /// </summary>
    public partial class CreateGame : UserControl, IGameScreen
    {
        int LeagueID = 0;
        List<Team> LeagueTeams;
        List<League> Leagues;
        TeamAdapter ta = new TeamAdapter();

        public GameScreenSetup SetupData { get; set; }

        private class SimpleTeamListItem
        {
            public int Id { get; set; }
            public string Name { get; set; }
        }

        public CreateGame()
        {
            InitializeComponent();

            for(int i = 1; i<= 31; i++)
            {
                cboDOBDay.Items.Add(i.ToString());
            }
            for (int i = 1; i <= 12; i++)
            {
                cboDOBMonth.Items.Add(new DateTime(1979, i, 1).ToString("MMMM", CultureInfo.InvariantCulture));
            }
            for (int i = 1900; i <= DateTime.Now.Year; i++)
            {
                cboDOBYear.Items.Add(i.ToString());
            }

            LeagueAdapter la = new LeagueAdapter();
            Leagues = la.GetLeagues();

            foreach(League l in Leagues)
            {
                LeaguePaging.Items.Add(new PagingItem(){ ID = l.UniqueID, Name = l.Name });
            }

            LeaguePaging.DisplayItem(0);

            
            SetupList();
            UpdateTeams();
        }


        private void SetupList()
        {
            lstTeams.Title = "";
            List<ListColumn> c = new List<ListColumn>();
            c.Add(new ListColumn("Team", 200));
            c.Add(new ListColumn("Av Rating", 100,HorizontalAlignment.Right));

            lstTeams.Columns = c;
        }

        private void UpdateTeams()
        {
            LeagueID = LeaguePaging.Items[LeaguePaging.CurrentItem].ID;
            LeagueTeams = (from tl in ta.GetTeamsByLeague(LeagueID)
                           orderby tl.Name
                           select tl).ToList();

            List<ListRow> rows = new List<ListRow>();

            foreach (Team t in LeagueTeams)
            {
                List<string> rowData = new List<string>();
                rowData.Add(t.Name);
                rowData.Add(ta.AveragePlayerRating(t.UniqueID).ToString("0.00"));

                rows.Add(new ListRow(t.UniqueID, rowData));
            }

            lstTeams.Rows = rows;
            lstTeams.Render();
        }

        private void LeaguePaging_EitherDirectionClicked(object sender, EventArgs e)
        {
            UpdateTeams();
        }

        private void lstTeams_RowClicked(object sender, EventArgs e)
        {
            ListRow sel = (ListRow)sender;
            Team t = ta.GetTeam(lstTeams.SelectedID);
            if(t != null)
            {
                MessageBox.Show(t.Name + " " + ta.GetPlayersInTeam(t.UniqueID)[0].Name);
            }
            
        }

        public ScreenReturnData MainButtonClick(int buttonId)
        {
            if(buttonId != 0)
            {
                throw new NotImplementedException();
            }

            ScreenReturnData ValidationResult = ValidateInput();
            if(ValidationResult != null)
            {
                return ValidationResult;
            }
            

            Manager you = new Manager();
            you.FirstName = txtFirstName.Text;
            you.LastName = txtSurname.Text;
            you.Human = true;
            you.DateOfBirth = new DateTime(Convert.ToInt32(cboDOBDay.Text), cboDOBMonth.SelectedIndex + 1, Convert.ToInt32(cboDOBDay.Text));
            you.Reputation = 50;

            ManagerAdapter ma = new ManagerAdapter();
            int ManagerID = ma.AddManager(you);
            Team T = ta.GetTeam(lstTeams.SelectedID);
            ma.AssignToTeam(ManagerID, T.UniqueID);

            MessageBox.Show("Assigned you to " + T.Name);
            return new ScreenReturnData(ScreenReturnCode.Ok); 
        }

        ScreenReturnData ValidateInput()
        {
            if (txtFirstName.Text.Trim() == "" || txtSurname.Text.Trim() == "")
            {
                return new ScreenReturnData(ScreenReturnCode.Error, "Please enter your name.");
            }

            if(lstTeams.SelectedID == -1)
            {
                return new ScreenReturnData(ScreenReturnCode.Error, "Please select a team to manage.");
            }

            DateTime dob;
            try
            {
                dob = new DateTime(Convert.ToInt32(cboDOBDay.Text), cboDOBMonth.SelectedIndex + 1, Convert.ToInt32(cboDOBDay.Text));
            }
            catch (Exception)
            {
                return new ScreenReturnData(ScreenReturnCode.Error, "Please enter a valid date of birth.");
            }

            return null;
        }

        public void SetupGameScreenData(GameScreenSetup dataFromUI)
        {
            SetupData = dataFromUI;
            SetupData.MainButtons.Add("Ok");

            SetupData.ShowContinueButton = false;
            SetupData.ShowDate = false;

            SetupData.Title1 = "Create New Game";
            SetupData.Title2 = "Welcome to GoalLine";
        }

        private void TestList_RowClicked(object sender, EventArgs e)
        {
            ListRow r = (ListRow)sender;

            MessageBox.Show(r.ColumnData[0]);
        }
    }
}
