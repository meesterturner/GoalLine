using System;
using System.Collections.Generic;
using System.Globalization;
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
using GoalLine.Data;
using GoalLine.Structures;
using GoalLine.UI.GameScreens;
using GoalLine.UI.Controls;
using System.Windows.Shell;
using System.Runtime.InteropServices.WindowsRuntime;

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

            UpdateTeams();

        }

        private void UpdateTeams()
        {
            LeagueID = LeaguePaging.Items[LeaguePaging.CurrentItem].ID;
            LeagueTeams = ta.GetTeamsByLeague(LeagueID);
            lvwTeams.ItemsSource = LeagueTeams;
        }

        private void LeaguePaging_EitherDirectionClicked(object sender, EventArgs e)
        {
            UpdateTeams();
        }

        private void lvwTeams_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Team t = (Team)lvwTeams.SelectedItem;
            MessageBox.Show(t.Name + " " + ta.GetPlayersInTeam(t.UniqueID)[0].Name);
        }

        public ScreenReturnData MainButtonClick(int buttonId) //TODO: Probably needs to return something!
        {
            if(buttonId != 0)
            {
                throw new NotImplementedException();
            }

            //TODO: Must do some validation
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
            Team T = (Team)lvwTeams.SelectedItem;
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

            if(lvwTeams.SelectedItem == null)
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

            SetupData.Title1 = "Create New Game";
            SetupData.Title2 = "Welcome to GoalLine";
        }
    }
}
