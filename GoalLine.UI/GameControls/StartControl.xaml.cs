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
using GoalLine.Data;
using GoalLine.Structures;
using GoalLine.UI.SingleControls;

namespace GoalLine.UI
{
    /// <summary>
    /// Interaction logic for StartControl.xaml
    /// </summary>
    public partial class StartControl : UserControl
    {
        int LeagueID = 0;
        List<Team> LeagueTeams;
        List<League> Leagues;
        TeamAdapter ta = new TeamAdapter();

        private class SimpleTeamListItem
        {
            public int Id { get; set; }
            public string Name { get; set; }
        }

        public StartControl()
        {
            InitializeComponent();

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
    }
}
