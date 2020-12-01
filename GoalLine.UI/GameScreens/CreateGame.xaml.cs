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
using GoalLine.UI.Utils;
using GoalLine.Resources;

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

            SetupText();
            SetupList();
            UpdateTeams();
        }

        private void SetupText()
        {
            lblFirstName.Text = LangResources.CurLang.FirstName + ":";
            lblSurname.Text = LangResources.CurLang.LastName + ":";
            lblDOB.Text = LangResources.CurLang.DateOfBirth + ":";
        }

        private void SetupList()
        {
            lstTeams.Title = "";
            lstTeams.Columns = new List<ListColumn>()
            {
                new ListColumn(LangResources.CurLang.Team, 200),
                new ListColumn(LangResources.CurLang.AverageRating, 150)
            };
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
                //rows.Add(new ListRow(t.UniqueID, new List<object>()
                //{
                //    t.Name,
                //    ta.AveragePlayerRating(t.UniqueID).ToString("0.00")
                //}));

                //double avgPlayer = ta.AveragePlayerRating(t.UniqueID);

                rows.Add(new ListRow(t.UniqueID, new List<object>()
                {
                    t.Name,
                    GraphicUtils.StarRating(ta.AveragePlayerRating(t.UniqueID) / 20)
                }));
            }

            lstTeams.Rows = rows;
        }

        private void LeaguePaging_EitherDirectionClicked(object sender, EventArgs e)
        {
            UpdateTeams();
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

            return new ScreenReturnData(ScreenReturnCode.Ok);
        }

        ScreenReturnData ValidateInput()
        {
            if (txtFirstName.Text.Trim() == "" || txtSurname.Text.Trim() == "")
            {
                return new ScreenReturnData(ScreenReturnCode.Error, LangResources.CurLang.PleaseEnterYourName);
            }

            if(lstTeams.SelectedID == -1)
            {
                return new ScreenReturnData(ScreenReturnCode.Error, LangResources.CurLang.PleaseSelectATeamToManage);
            }

            DateTime dob;
            try
            {
                dob = new DateTime(Convert.ToInt32(cboDOBDay.Text), cboDOBMonth.SelectedIndex + 1, Convert.ToInt32(cboDOBDay.Text));
            }
            catch (Exception)
            {
                return new ScreenReturnData(ScreenReturnCode.Error, LangResources.CurLang.PleaseEnterDateOfBirth);
            }

            return null;
        }

        public void SetupGameScreenData(GameScreenSetup dataFromUI)
        {
            SetupData = dataFromUI;
            SetupData.MainButtons.Add(LangResources.CurLang.OK);

            SetupData.ShowContinueButton = false;
            SetupData.ShowDate = false;

            SetupData.Title1 = LangResources.CurLang.StartNewGame;
            SetupData.Title2 = LangResources.CurLang.WelcomeToGoalLine;
        }
    }
}
