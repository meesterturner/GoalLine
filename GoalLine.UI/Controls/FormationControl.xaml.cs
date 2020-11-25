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
using GoalLine.Structures;
using GoalLine.Data;

namespace GoalLine.UI.Controls
{
    

    /// <summary>
    /// Interaction logic for FormationControl.xaml
    /// </summary>
    public partial class FormationControl : UserControl
    {

        const int GRIDWIDTH = 5; // 0-4
        const int GRIDHEIGHT = 8; // 0-7

        Ellipse[,] Markers = new Ellipse[GRIDWIDTH, GRIDHEIGHT];
        TextBlock[,] MarkerText = new TextBlock[GRIDWIDTH, GRIDHEIGHT];
        int[,] PlayerGridPositions = new int[GRIDWIDTH, GRIDHEIGHT];
        List<TextBlock> PlayerLabels = new List<TextBlock>();

        private Team _team;
        public Team team {
            private get
            {
                return _team;
            }
            set 
            {
                _team = value;
                SetupTeam();
            }
        }

        private void SetupTeam()
        {
            PlayerAdapter pa = new PlayerAdapter();
            
            foreach(KeyValuePair<int, TeamPlayer> p in team.Players)
            {
                TextBlock playerLabel = new TextBlock();
                playerLabel.Width = 150;
                playerLabel.Height = 30;
                playerLabel.Text = pa.GetPlayer(p.Value.PlayerID).DisplayName(PersonNameReturnType.LastnameInitial);
                playerLabel.MouseMove += new MouseEventHandler(PlayerName_MouseMove);
                playerLabel.Tag = p.Value.PlayerID;

                PlayerLabels.Add(playerLabel);

                stkNames.Children.Add(PlayerLabels[PlayerLabels.Count - 1]);
                playerLabel = null;

            }
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

            if(e.Data.GetDataPresent(DataFormats.StringFormat))
            {
                int PlayerID = Convert.ToInt32((string)e.Data.GetData(DataFormats.StringFormat)); // Should be player ID
                Ellipse DropControl = (Ellipse)sender;
                //TextBlock DropControl = (TextBlock)sender;

                string[] coords = DropControl.Tag.ToString().Split(new string[] { "," }, StringSplitOptions.None);
                int x = Convert.ToInt32(coords[0]);
                int y = Convert.ToInt32(coords[1]);

                PlayerAdapter pa = new PlayerAdapter();
                MarkerText[x, y].Text = pa.GetPlayer(PlayerID).DisplayName(PersonNameReturnType.LastnameInitialOptional);
                MarkerText[x, y].Visibility = Visibility.Visible;

                PlayerGridPositions[x, y] = PlayerID;
            }
        }

        public FormationControl()
        {
            InitializeComponent();
            SetupControl();
            SetupFormationTemplate("442");
        }



        private void SetupControl()
        {
            // Create empty circles inside the grid
            // Set the PlayerGridPositions to blanks
            for(int x = 0; x < GRIDWIDTH; x++)
            {
                for(int y = 0; y < GRIDHEIGHT; y++)
                {
                    // --- Marker symbol ---
                    Markers[x, y] = new Ellipse();
                    Markers[x, y].Height = 50;
                    Markers[x, y].Width = 50;
                    Markers[x, y].Stroke = Brushes.DarkGray;
                    Markers[x, y].Fill = Brushes.Transparent;
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
                    MarkerText[x, y].Foreground = Brushes.Black;
                    MarkerText[x, y].FontSize = 10;
                    MarkerText[x, y].VerticalAlignment = VerticalAlignment.Center;
                    MarkerText[x, y].HorizontalAlignment = HorizontalAlignment.Center;

                    Grid.SetColumn(MarkerText[x, y], x + 1);
                    Grid.SetRow(MarkerText[x, y], 8 - y);
                    
                    grdPitch.Children.Add(MarkerText[x, y]);

                    PlayerGridPositions[x, y] = -1;
                }
            }

            grdPitch.ShowGridLines = true;
        }

        public void SetupFormationTemplate(string Formation)
        {
            for(int x = 0; x < GRIDWIDTH; x++)
            {
                for (int y = 0; y < GRIDHEIGHT; y++)
                {
                    SetMarkerTemplate(x, y, false);
                }
            }

            List<Point> points = new List<Point>();
            // Goalkeeper always same
            points.Add(new Point(2, 0));

            // Our best rendition of a 4-4-2
            points.Add(new Point(0, 1));
            points.Add(new Point(1, 1));
            points.Add(new Point(3, 1));
            points.Add(new Point(4, 1));

            points.Add(new Point(0, 3));
            points.Add(new Point(1, 3));
            points.Add(new Point(3, 3));
            points.Add(new Point(4, 3));

            points.Add(new Point(1, 6));
            points.Add(new Point(3, 6));

            foreach(Point p in points)
            {
                SetMarkerTemplate((int)p.X, (int)p.Y, true);
            }
        }

        private void SetMarkerTemplate(int x, int y, bool set)
        {
            Markers[x, y].Fill = (set ? Brushes.LightGreen : Brushes.Transparent);
        }
    }
}
