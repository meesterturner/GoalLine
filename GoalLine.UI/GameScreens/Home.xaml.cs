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
using GoalLine.Resources;
using GoalLine.UI.Utils;

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
            SetupText();
        }

        private void SetupText()
        {
            lblFrom.Text = LangResources.CurLang.From + ":";
            lblDate.Text = LangResources.CurLang.Date + ":";
            lblSubject.Text = LangResources.CurLang.Subject + ":";
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

            UpdateEmails();
            UpdateLeagueTable();
        }

        private void UpdateEmails()
        {
            EmailAdapter ea = new EmailAdapter();
            lstEmails.Title = LangResources.CurLang.Emails;
            lstEmails.Columns = new List<ListColumn>()
            {
                new ListColumn(LangResources.CurLang.Unread, 35),
                new ListColumn(LangResources.CurLang.Date, 125),
                new ListColumn(LangResources.CurLang.Subject, 390)
            };

            List<ListRow> rows = new List<ListRow>();
            List<Email> emails = ea.GetEmails(SetupData.ManagerData.UniqueID);
            foreach(Email e in emails)
            {
                EmailViewable ev = ea.ConvertEmailToViewable(e);

                rows.Add(new ListRow(e.UniqueID, new List<object>() { 
                    (ev.Read ? null : GraphicUtils.StarRating(1)),
                    ev.Date.ToString(LangResources.CurLang.DateFormat),
                    ev.Subject
                }));
            }

            lstEmails.Rows = rows;
            lstEmails.SelectionMode = SelectMode.HighlightAndCallback;
            lstEmails.Callback_ItemClick = ShowEmail;
        }

        private void ShowEmail()
        {

            int id = lstEmails.SelectedID;
            if(id > -1)
            {
                EmailAdapter ea = new EmailAdapter();

                EmailViewable ev = ea.ConvertEmailToViewable(ea.GetEmail(SetupData.ManagerData.UniqueID, id));

                lblFromDetail.Text = ev.From;
                lblDateDetail.Text = ev.Date.ToString(LangResources.CurLang.DateFormat);
                lblSubjectDetail.Text = ev.Subject;
                lblEmail.Text = ev.Body;

                ea.MarkEmailAsRead(SetupData.ManagerData.UniqueID, ev.UniqueID, true);
            }
        }

        private void UpdateLeagueTable()
        {
            // Headings[] should contain: Pos,Team,P,W,D,L,F,A,GD,Pts, or equivalent
            string[] Headings = LangResources.CurLang.LeagueTableHeadings.Split(new string[] { "," }, StringSplitOptions.None);

            LeagueAdapter la = new LeagueAdapter();
            lstLeague.Title = la.GetLeague(SetupData.TeamData.LeagueID).Name;
            lstLeague.Columns = new List<ListColumn>()
            {
                new ListColumn(Headings[0], 75),
                new ListColumn(Headings[1], 300),
                new ListColumn(Headings[2], 75, HorizontalAlignment.Right),
                new ListColumn(Headings[3], 75, HorizontalAlignment.Right),
                new ListColumn(Headings[4], 75, HorizontalAlignment.Right),
                new ListColumn(Headings[5], 75, HorizontalAlignment.Right),
                new ListColumn(Headings[6], 75, HorizontalAlignment.Right),
                new ListColumn(Headings[7], 75, HorizontalAlignment.Right),
                new ListColumn(Headings[8], 75, HorizontalAlignment.Right),
                new ListColumn(Headings[9], 75, HorizontalAlignment.Right),
            };


            List<LeagueTableRecord> table = la.LeagueTable(SetupData.TeamData.LeagueID);

            List<ListRow> rows = new List<ListRow>();

            for(int i = 0; i < table.Count(); i++)
            {
                LeagueTableRecord t = table[i];

                rows.Add(new ListRow(t.TeamID, new List<object>() {
                    (i + 1).ToString(),
                    t.Name,
                    t.SeasonStatistics.GamesPlayed.ToString(),
                    t.SeasonStatistics.Won.ToString(),
                    t.SeasonStatistics.Drawn.ToString(),
                    t.SeasonStatistics.Lost.ToString(),
                    t.SeasonStatistics.GoalsScored.ToString(),
                    t.SeasonStatistics.GoalsConceded.ToString(),
                    t.SeasonStatistics.GoalDifference.ToString(),
                    t.SeasonStatistics.Points.ToString()
                }));
            }

            lstLeague.Rows = rows;
        }

        public ScreenReturnData MainButtonClick(int buttonId)
        {
            throw new NotImplementedException();

            //return new ScreenReturnData(ScreenReturnCode.Ok);
        }

        public ScreenReturnData ContinueButtonClick()
        {
            throw new NotImplementedException();
        }
    }
}
