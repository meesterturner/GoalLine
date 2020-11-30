using System;
using System.Collections.Generic;
using System.Windows;
using GoalLine.Data;
using GoalLine.Structures;
using GoalLine.UI.Controls;
using GoalLine.UI.Utils;
using GoalLine.Resources;

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
            imgLogo.Source = ImageResources.GetImage(ImageResourceList.Logo);
        }

        private void cmdStart_Click(object sender, RoutedEventArgs e)
        {
            StartGame(null);
            
        }

        private void SetupList()
        {
            GameIO io = new GameIO();
            Saves = io.ListSaveGames();

            lstSaves.Title = "Saved Games";

            lstSaves.Columns = new List<ListColumn>() {
                new ListColumn("Name", 300),
                new ListColumn("Date", 200, HorizontalAlignment.Right)
            };

            List<ListRow> rows = new List<ListRow>();

            for (int s = 0; s < Saves.Count; s++)
            {
                rows.Add(new ListRow(s, new List<object>() {
                    Saves[s].Name,
                    Saves[s].SaveDate.ToString("dd/MM/yyyy HH:mm")
                }));
            }

            lstSaves.Rows = rows;
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
            UiUtils.MainWindowGrid = g.grdMain;
            g.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            g.WindowState = WindowState.Maximized;
            g.Show();
            g.StartGame(FromSave);

            this.Close();
        }

        private void cmdLoad_Click(object sender, RoutedEventArgs e)
        {
            if(lstSaves.SelectedID == -1 )
            {
                UiUtils.OpenDialogBox(grdMain, "Unable to Load Game", "You have not selected a game to load.", new List<DialogButton>() {
                    new DialogButton("Ok", null, null)
                });
                return;
            }

            string SaveGameName = Saves[lstSaves.SelectedID].Name;
            StartGame(SaveGameName);
        }

        private void cmdQuit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
