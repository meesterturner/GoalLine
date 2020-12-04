using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
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
        private List<Grid> playerBlocks = new List<Grid>();

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
            switch(buttonId)
            {
                case 1:
                    SetupData.Parent.ShowTacticsScreen(SetupData.TeamData);
                    return new ScreenReturnData(ScreenReturnCode.None);

                case 0:
                    return new ScreenReturnData(ScreenReturnCode.Cancel);

                default:
                    throw new NotImplementedException();
            }
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
            
            SetupData.ShowContinueButton = MyTeam;
            SetupData.MainButtons.Add(LangResources.CurLang.Tactics);
            SetupData.MainButtons.Add(LangResources.CurLang.Back);
            

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
            GeneratePlayerBlocks(p);
        }

        

        private void GeneratePlayerBlocks(Player p)
        {
            if(playerBlocks.Count() > 0)
            {
                foreach(Grid g in playerBlocks)
                {
                    UiUtils.RemoveControl(g);
                }

                playerBlocks.Clear();
            }

            AddBlock(GeneratePlayerTitleBlock(p), grdMain, 1, 0);
            AddBlock(GeneratePlayerStatsBlock(p), stkInfo1);
            AddBlock(GeneratePlayerContractBlock(p), stkInfo1);
            AddBlock(GeneratePlayerMedicalBlock(p), stkInfo2);
            AddBlock(GeneratePlayerTrainingBlock(p), stkInfo2);
        }

        private void AddBlock(Grid block, Grid dest, int col, int row)
        {
            Grid.SetColumn(block, col);
            Grid.SetRow(block, row);
            dest.Children.Add(block);
            playerBlocks.Add(block);
        }

        private void AddBlock(Grid block, StackPanel dest)
        {
            dest.Children.Add(block);
            playerBlocks.Add(block);
        }

        private Grid GeneratePlayerTitleBlock(Player p)
        {
            PlayerAdapter pa = new PlayerAdapter();
            Data.Utils u = new Data.Utils();

            Grid g;
            g = GenerateBlankBlockGrid(3, p.DisplayName(PersonNameReturnType.FirstnameLastname), 2);
            Grid.SetColumnSpan(g, 2);

            TextBlock t = new TextBlock();
            t.Text = pa.PositionAndSideText(p, false);
            t.Style = Application.Current.FindResource("ListHeader") as Style;
            t.Margin = new Thickness(8, 0, 0, 0);
            Grid.SetColumn(t, 0);
            Grid.SetColumnSpan(t, 2);
            Grid.SetRow(t, 1);
            g.Children.Add(t);

            UiUtils.AddGridData(g, 0, 2, LangResources.CurLang.DateOfBirth, 
                p.DateOfBirth.ToString(LangResources.CurLang.DateFormat) + string.Format(" (Age: {0})", u.CalculateAgeInGame(p.DateOfBirth)));

            StackPanel stars = GraphicUtils.StarRating(p.Stars);
            stars.HorizontalAlignment = HorizontalAlignment.Right;
            Grid.SetColumn(stars, 3);
            Grid.SetColumnSpan(stars, 3);
            Grid.SetRow(stars, 0);
            g.Children.Add(stars);
            return g;
        }

        private Grid GeneratePlayerStatsBlock(Player p)
        {
            Grid g;
            g = GenerateBlankBlockGrid(3, "Player Attributes", 1);

            List<(string, int)> stats = new List<(string, int)>();
            stats.Add(("Agility", p.Agility));
            stats.Add(("Attitude", p.Attitude));
            stats.Add(("Speed", p.Speed));
            stats.Add(("Stamina", p.Stamina));

            List<(string, int)> statsOrdered = (from s in stats 
                                                orderby s.Item1 
                                                select s).ToList(); // Because localised, these may not be in alpha-order!

            int halfList = (int)Math.Ceiling((double)stats.Count() / 2) - 1;
            for(int i = 0; i <= halfList; i++)
            {
                UiUtils.AddGridData(g, 0, i + 1, statsOrdered[i].Item1, statsOrdered[i].Item2);
                UiUtils.AddGridData(g, 2, i + 1, statsOrdered[i + halfList + 1].Item1, statsOrdered[i + halfList + 1].Item2);
            }

            
            return g;
        }

        private Grid GeneratePlayerContractBlock(Player p)
        {
            Grid g;
            g = GenerateBlankBlockGrid(4, "Contract", 1);

            UiUtils.AddGridData_DoubleSize(g, 0, 1, "Date Ending", "--/--/----");
            UiUtils.AddGridData_DoubleSize(g, 0, 2, "Value", "£" + p.Value.ToString("N0"));
            UiUtils.AddGridData_DoubleSize(g, 0, 3, "Wages (per week)", "£" + p.Wages.ToString("N0"));
            return g;
        }

        private Grid GeneratePlayerMedicalBlock(Player p)
        {
            Grid g;
            g = GenerateBlankBlockGrid(4, "Medical", 1);

            UiUtils.AddGridData_DoubleSize(g, 0, 1, "Physical Condition", "--");
            UiUtils.AddGridData_DoubleSize(g, 0, 2, "Injury", "None");
            UiUtils.AddGridData_DoubleSize(g, 0, 3, "Est. Availability", "n/a");
            return g;
        }

        private Grid GeneratePlayerTrainingBlock(Player p)
        {
            Grid g;
            g = GenerateBlankBlockGrid(4, "Training", 1);

            return g;
        }

        private Grid GenerateBlankBlockGrid(int rows, string title, int span)
        {
            int[] colWidths = new int[] { 125, 75, 125, 75 };
            int totWidth = 0;

            Grid g = new Grid();
            for(int i = 1; i <= rows; i++)
            {
                g.RowDefinitions.Add(new RowDefinition());
            }

            for (int i = 1; i <= colWidths.Count(); i++)
            {
                int thisWidth = colWidths[i - 1] * span;
                totWidth += thisWidth;

                g.ColumnDefinitions.Add(new ColumnDefinition());
                g.ColumnDefinitions[i - 1].Width = new GridLength(thisWidth);
            }

            g.Height = (rows * 25) + 10;
            g.Margin = new Thickness(0, 0, 0, 3);

            Rectangle r = new Rectangle();
            r.Opacity = 0.5;
            r.Width = totWidth;
            r.Height = 2000;
            r.Fill = Brushes.White;
            Grid.SetColumnSpan(r, 4 * span);
            Grid.SetRowSpan(r, rows);
            g.Children.Add(r);

            TextBlock t = new TextBlock();
            t.Text = title;
            t.Style = Application.Current.FindResource("DialogTitle") as Style;
            Grid.SetColumnSpan(t, 4);
            g.Children.Add(t);

            return g;
        }

        
    }
}
