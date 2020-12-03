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
using GoalLine.Resources;
using GoalLine.Structures;
using GoalLine.UI.Controls;
using GoalLine.UI.Utils;

namespace GoalLine.UI.GameScreens
{
    /// <summary>
    /// Interaction logic for TeamInfo.xaml
    /// </summary>
    public partial class TeamInfo : UserControl, IGameScreen
    {
        private bool MyTeam = false;

        public TeamInfo()
        {
            InitializeComponent();
        }

        public GameScreenSetup SetupData { get; set; }

        public ScreenReturnData ContinueButtonClick()
        {
            return new ScreenReturnData(ScreenReturnCode.Ok);
        }

        public ScreenReturnData MainButtonClick(int buttonId)
        {
            throw new NotImplementedException();
        }

        public void SetupGameScreenData(GameScreenSetup dataFromUI)
        {
            if (dataFromUI.TeamData == null)
            {
                throw new Exception("TeamData is null");
            }

            ManagerAdapter ma = new ManagerAdapter();
            WorldAdapter wa = new WorldAdapter();

            SetupData = dataFromUI;
            SetupData.ManagerData = ma.GetManager(SetupData.TeamData.ManagerID);

            MyTeam = (wa.CurrentManagerID == SetupData.TeamData.ManagerID);
            
            SetupData.ShowContinueButton = true;

            SetupData.Title1 = SetupData.TeamData.Name;
            SetupData.Title2 = SetupData.ManagerData.Name;

            UpdatePlayers();
        }

        private void UpdatePlayers()
        {
            PlayerAdapter pa = new PlayerAdapter();
            lstPlayers.Title = LangResources.CurLang.Players;
            lstPlayers.Columns = new List<ListColumn>()
            {
                new ListColumn(LangResources.CurLang.Name, 200),
                new ListColumn(LangResources.CurLang.Position_Short, 50),
                new ListColumn(LangResources.CurLang.Rating, 120)
            };

            List<ListRow> rows = new List<ListRow>();
            List<Player> players = (from p in pa.GetPlayers(SetupData.TeamData.UniqueID)
                                    orderby p.Position, p.PreferredSide
                                    select p).ToList();

            foreach (Player p in players)
            {
                rows.Add(new ListRow(p.UniqueID, new List<object>() {
                    p.DisplayName(PersonNameReturnType.LastnameInitial),
                    pa.PositionAndSideText(p, true),
                    GraphicUtils.StarRating(p.Stars)
                }));
            }

            lstPlayers.Rows = rows;
            lstPlayers.SelectionMode = SelectMode.HighlightAndCallback;
            lstPlayers.Callback_ItemClick = ShowPlayer;
        }

        private void ShowPlayer()
        {
            int id = lstPlayers.SelectedID;

            if(id == -1)
            {
                throw new Exception("No player selected");
            }

            PlayerAdapter pa = new PlayerAdapter();
            Player p = pa.GetPlayer(lstPlayers.SelectedID);

            lblName.Text = p.DisplayName(PersonNameReturnType.FirstnameLastname);
            lblPosition.Text = pa.PositionAndSideText(p, false);
            lblValue.Text = string.Format("Value: £{0:n0}", p.Value.ToString());

            lstSelected.Title = "";

            lstSelected.Columns = new List<ListColumn>()
            {
                new ListColumn("attrib", 100),
                new ListColumn("bar", 50),
                new ListColumn("val", 120, HorizontalAlignment.Right)
            };

            // "Position", "PreferredSide", "DateOfBirth", "", 

            string[] DisplayAttributes = new string[] {"Agility", "Attitude", "Speed", "Stamina", "", "Wages" };
            List<ListRow> rows = new List<ListRow>();

            for (int a = 0; a <= DisplayAttributes.GetUpperBound(0); a++)
            {
                string attrib = DisplayAttributes[a];

                if (attrib != "")
                {
                    int value = Convert.ToInt32(p.GetType().GetProperty(attrib).GetValue(p));

                    rows.Add(new ListRow(a, new List<object>() { 
                        attrib,
                        "?",
                        value.ToString()
                    }));
                }
                else
                {
                    rows.Add(new ListRow(a, new List<object>() {
                        " ",
                        " ",
                        " "
                    }));
                }

            }

            lstSelected.Rows = rows;
            lstSelected.SelectionMode = SelectMode.Highlight; // TODO: Need a "none!"
        }
    }
}
