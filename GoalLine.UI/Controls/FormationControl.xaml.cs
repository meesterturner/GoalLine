﻿using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using GoalLine.Structures;
using GoalLine.Data;
using GoalLine.Resources;
using GoalLine.UI.Utils;

namespace GoalLine.UI.Controls
{
    

    /// <summary>
    /// Interaction logic for FormationControl.xaml
    /// </summary>
    public partial class FormationControl : UserControl
    {

        const int GRIDWIDTH = 5; // 0-4
        const int GRIDHEIGHT = 8; // 0-7

        Polygon[,] Markers = new Polygon[GRIDWIDTH, GRIDHEIGHT];
        TextBlock[,] MarkerText = new TextBlock[GRIDWIDTH, GRIDHEIGHT];
        int[,] PlayerGridPositions = new int[GRIDWIDTH, GRIDHEIGHT];
        List<TextBlock> PlayerLabels = new List<TextBlock>();

        bool MyTeam;

        private Team _team;
        public Team team {
            private get
            {
                return _team;
            }
            set 
            {
                _team = value;

                WorldAdapter wa = new WorldAdapter();
                MyTeam = (wa.CurrentManagerID == _team.ManagerID);

                SetupTeam(true);
            }
        }

        public int CurrentFormationID { get; private set; }
        public bool ChangesNotSaved { get; private set; }

        public void SaveFormation()
        {
            TeamAdapter ta = new TeamAdapter();
            ta.SavePlayerFormation(team.UniqueID, CurrentFormationID, PlayerGridPositions);
            
            UiUtils.OpenDialogBox(UiUtils.MainWindowGrid, LangResources.CurLang.Formation, LangResources.CurLang.FormationSavedSuccessfully, new List<DialogButton>() { 
                new DialogButton(LangResources.CurLang.OK, null, null)
            });

            ChangesNotSaved = false;
        }

        public void SetupTeam(bool WithLabels)
        {
            int FormationID = (MyTeam ? team.CurrentFormation : team.LastKnownFormation);
            Dictionary<int, TeamPlayer> picks = (MyTeam ? team.Players : team.LastKnownPick);

            FormationPaging.DisplayItem(FormationID);
            SetupFormationTemplate(FormationID);

            PlayerAdapter pa = new PlayerAdapter();
            
            foreach(KeyValuePair<int, TeamPlayer> p in picks)
            {
                TeamPlayer tp = p.Value;
                Player player = pa.GetPlayer(tp.PlayerID);

                if (WithLabels)
                {
                    StackPanel s = new StackPanel();
                    s.Orientation = Orientation.Horizontal;

                    TextBlock playerLabel = new TextBlock();
                    playerLabel.Width = 150;
                    playerLabel.Height = 30;
                    playerLabel.Text = player.DisplayName(PersonNameReturnType.LastnameInitial);
                    playerLabel.MouseMove += new MouseEventHandler(PlayerName_MouseMove);
                    playerLabel.Tag = tp.PlayerID;
                    playerLabel.VerticalAlignment = VerticalAlignment.Center;
                    playerLabel.Cursor = Cursors.Hand;
                    PlayerLabels.Add(playerLabel);

                    s.Children.Add(PlayerLabels[PlayerLabels.Count - 1]);

                    TextBlock playerPos = new TextBlock();
                    playerPos.Width = 60;
                    playerPos.Height = 30;
                    playerPos.Text = pa.PositionAndSideText(player, true);
                    playerPos.VerticalAlignment = VerticalAlignment.Center;

                    s.Children.Add(playerPos);

                    s.Children.Add(GraphicUtils.StarRatingWithNumber(player.Stars));

                    //stkNames.Children.Add(PlayerLabels[PlayerLabels.Count - 1]);
                    stkNames.Children.Add(s);
                    playerLabel = null;
                }
                


                if(tp.Selected == PlayerSelectionStatus.Starting && tp.PlayerGridX > -1 && tp.PlayerGridY > -1)
                {
                    MarkerText[tp.PlayerGridX, tp.PlayerGridY].Text = player.DisplayName(PersonNameReturnType.InitialOptionalLastname);
                    MarkerText[tp.PlayerGridX, tp.PlayerGridY].Visibility = Visibility.Visible;
                    PlayerGridPositions[tp.PlayerGridX, tp.PlayerGridY] = tp.PlayerID;
                 }
            }

            ChangesNotSaved = false;
        }

        private void PlayerName_MouseMove(object sender, MouseEventArgs e)
        {
            TextBlock name = (TextBlock)sender;

            base.OnMouseMove(e);
            if(e.LeftButton == MouseButtonState.Pressed)
            {
                //https://www.youtube.com/watch?v=AqJLW-6dxfA&ab_channel=codefactory2016 for drag and drop shiz

                // Package the data.
                DataObject data = new DataObject();
                data.SetData(DataFormats.StringFormat, name.Tag.ToString());
                //data.SetData("Double", circleUI.Height);
                data.SetData("Object", this);

                // Inititate the drag-and-drop operation. 
                DragDrop.DoDragDrop(this, data, DragDropEffects.Copy | DragDropEffects.Move);
            }
        }

        private void Marker_Drop(object sender, DragEventArgs e)
        {
            base.OnDrop(e);

            if (e.Data.GetDataPresent(DataFormats.StringFormat))
            {
                int PlayerID = Convert.ToInt32((string)e.Data.GetData(DataFormats.StringFormat)); // Should be player ID
                Polygon DropControl = (Polygon)sender;
                //TextBlock DropControl = (TextBlock)sender;

                string[] coords = DropControl.Tag.ToString().Split(new string[] { "," }, StringSplitOptions.None);
                int xPos = Convert.ToInt32(coords[0]);
                int yPos = Convert.ToInt32(coords[1]);

                PlayerAdapter pa = new PlayerAdapter();
                MarkerText[xPos, yPos].Text = pa.GetPlayer(PlayerID).DisplayName(PersonNameReturnType.LastnameInitialOptional);
                MarkerText[xPos, yPos].Visibility = Visibility.Visible;

                PlayerGridPositions[xPos, yPos] = PlayerID;

                // Scan other positions to ensure we don't duplicate the player
                for (int x = 0; x < GRIDWIDTH; x++)
                {
                    for (int y = 0; y < GRIDHEIGHT; y++)
                    {
                        if (PlayerGridPositions[x, y] == PlayerID && !(x == xPos && y == yPos))
                        {
                            MarkerText[x, y].Visibility = Visibility.Hidden;
                            PlayerGridPositions[x, y] = -1;
                        }
                    }
                }

                ChangesNotSaved = true;
            }
        }

        public FormationControl()
        {
            InitializeComponent();
            SetupControl();
        }



        private void SetupControl()
        {
            imgPitch.ImageSource = ImageResources.GetImage(ImageResourceList.Pitch);

            // Paging control for formations
            FormationAdapter fa = new FormationAdapter();

            foreach (Formation f in fa.GetFormations())
            {
                FormationPaging.Items.Add(new PagingItem() { ID =  f.UniqueID, Name = f.Name });
            }


            // Create empty circles inside the grid
            // Set the PlayerGridPositions to blanks
            for (int x = 0; x < GRIDWIDTH; x++)
            {
                for(int y = 0; y < GRIDHEIGHT; y++)
                {
                    // --- Marker symbol ---
                    Markers[x, y] = GraphicUtils.Shirt();
                    Markers[x, y].VerticalAlignment = VerticalAlignment.Center;
                    Markers[x, y].HorizontalAlignment = HorizontalAlignment.Center;
                    Markers[x, y].AllowDrop = true;
                    Markers[x, y].Drop += new DragEventHandler(Marker_Drop);
                    Markers[x, y].Tag = x.ToString() + "," + y.ToString();

                    Grid.SetColumn(Markers[x, y], x + 1);
                    Grid.SetRow(Markers[x, y], 8 - y);
                    grdPitch.Children.Add(Markers[x, y]);

                    // --- Text ---
                    MarkerText[x, y] = new TextBlock();
                    MarkerText[x, y].Text = "";
                    MarkerText[x, y].Visibility = Visibility.Hidden;
                    MarkerText[x, y].Foreground = Brushes.White;
                    MarkerText[x, y].FontSize = 12;
                    MarkerText[x, y].FontFamily = new FontFamily("Roboto Black");

                    MarkerText[x, y].VerticalAlignment = VerticalAlignment.Center;
                    MarkerText[x, y].HorizontalAlignment = HorizontalAlignment.Center;

                    Grid.SetColumn(MarkerText[x, y], x + 1);
                    Grid.SetRow(MarkerText[x, y], 8 - y);
                    
                    grdPitch.Children.Add(MarkerText[x, y]);

                    PlayerGridPositions[x, y] = -1;
                }
            }
        }

        public void SetupFormationTemplate(int FormationID)
        {
            CurrentFormationID = FormationID;

            for (int x = 0; x < GRIDWIDTH; x++)
            {
                for (int y = 0; y < GRIDHEIGHT; y++)
                {
                    SetMarkerTemplate(x, y, false);
                    MarkerText[x, y].Visibility = Visibility.Hidden;
                    PlayerGridPositions[x, y] = -1;
                }
            }

            FormationAdapter fa = new FormationAdapter();
            List<Point2> points = fa.GetFormation(FormationID).Points;

            foreach(Point2 p in points)
            {
                SetMarkerTemplate((int)p.X, (int)p.Y, true);
            }
        }

        private void SetMarkerTemplate(int x, int y, bool set)
        {
            Markers[x, y].Visibility = (set ? Visibility.Visible : Visibility.Hidden);
        }

        private void cmdSave_Click(object sender, RoutedEventArgs e)
        {
            SaveFormation();
        }

        private void FormationPaging_EitherDirectionClicked(object sender, EventArgs e)
        {
            int FormationID = FormationPaging.Items[FormationPaging.CurrentItem].ID;
            SetupFormationTemplate(FormationID);
            ChangesNotSaved = true;
        }
    }
}
