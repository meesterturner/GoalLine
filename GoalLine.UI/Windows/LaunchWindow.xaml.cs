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
using System.Windows.Shapes;
using GoalLine.Data;
using GoalLine.Structures;
using GoalLine.Processes;
using GoalLine.UI.Controls;

namespace GoalLine.UI
{
    /// <summary>
    /// Interaction logic for LaunchWindow.xaml
    /// </summary>
    public partial class LaunchWindow : Window
    {
        List<SaveGameInfo> Saves;

        public LaunchWindow()
        {
            InitializeComponent();
            SetupList();
        }

        private void cmdStart_Click(object sender, RoutedEventArgs e)
        {
            StartGame(null);
            
        }

        private void SetupList()
        {
            GameIO io = new GameIO();
            Saves = io.ListSaveGames();

            lstSaves.Title = "Saved Games (Click to load)";

            List<ListColumn> c = new List<ListColumn>();
            c.Add(new ListColumn("Name", 500));
            c.Add(new ListColumn("Date", 200, HorizontalAlignment.Right));
            lstSaves.Columns = c;

            List<ListRow> rows = new List<ListRow>();

            for (int s = 0; s < Saves.Count; s++)
            {
                List<string> rowData = new List<string>();
                rowData.Add(Saves[s].Name);
                rowData.Add(Saves[s].SaveDate.ToString("dd/MM/yyyy hh:mm"));
                rows.Add(new ListRow(s, rowData));
            }

            lstSaves.Rows = rows;
            lstSaves.Render();
        }

        private void lstSaves_RowClicked(object sender, EventArgs e)
        {
            ListRow sel = (ListRow)sender;
            string SaveGameName = Saves[sel.ID].Name;
            StartGame(SaveGameName);
        }

        private void StartGame(string SaveGame)
        {
            bool FromSave;

            if(SaveGame == null)
            {
                Initialiser init = new Initialiser();
                init.CreateWorld();
                FromSave = false;
            } else
            {
                GameIO io = new GameIO();
                io.SaveGameName = SaveGame;
                io.LoadGame();
                FromSave = true;
            }


            GameWindow g = new GameWindow();
            g.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            g.WindowState = WindowState.Maximized;
            g.Show();
            g.StartGame(FromSave);

            this.Close();
        }
    }
}
