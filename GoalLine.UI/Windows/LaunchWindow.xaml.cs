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
            SetupText();
            SetupList();
            imgLogo.Source = ImageResources.GetImage(ImageResourceList.Logo);
        }

        private void SetupText()
        {
            cmdStart.Content = LangResources.CurLang.StartNewGame;
            cmdLoad.Content = LangResources.CurLang.LoadSavedGame;
            cmdQuit.Content = LangResources.CurLang.QuitGame;
        }

        private void cmdStart_Click(object sender, RoutedEventArgs e)
        {
            StartGame(null);
            
        }

        private void SetupList()
        {
            GameIO io = new GameIO();
            Saves = io.ListSaveGames();

            lstSaves.Title = LangResources.CurLang.SavedGames;

            lstSaves.Columns = new List<ListColumn>() {
                new ListColumn(LangResources.CurLang.Name, 300),
                new ListColumn(LangResources.CurLang.Date, 200, HorizontalAlignment.Right)
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

                try
                {
                    io.LoadGame();
                    
                }
                catch (Exception ex)
                {
                    UiUtils.OpenDialogBox(grdMain, LangResources.CurLang.LoadSavedGame, LangResources.CurLang.Error + ": " + ex.Message,
                        new List<DialogButton>() { new DialogButton(LangResources.CurLang.OK, null, null) });
                    return;
                }


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
                UiUtils.OpenDialogBox(grdMain, LangResources.CurLang.LoadSavedGame, LangResources.CurLang.YouHaveNotSelectedAGame, new List<DialogButton>() {
                    new DialogButton(LangResources.CurLang.OK, null, null)
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
