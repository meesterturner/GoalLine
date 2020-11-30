using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using GoalLine.Data;
using GoalLine.Structures;
using GoalLine.Processes;
using GoalLine.UI.GameScreens;
using GoalLine.Resources;
using GoalLine.UI.Utils;
using System.Windows.Shapes;
using System.Windows.Media;
using System.Windows.Input;

namespace GoalLine.UI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class GameWindow : Window
    {
        IGameScreen CurrentScreen;
        bool ShowingNewGame = false;
        bool SaveGameJustLoaded = false;

        List<Manager> HumanManagers;
        int PlayingHumanManager = 0;

        public GameWindow()
        {
            InitializeComponent();
            grdPopup.Visibility = Visibility.Hidden;
            imgLogo.Source = ImageResources.GetImage(ImageResourceList.LogoSmall);

        }

        public void StartGame(bool SaveGameLoaded)
        {
            SaveGameJustLoaded = SaveGameLoaded;
            CreateBackground();

            if (!SaveGameLoaded)
            {
                ShowingNewGame = true;
                ShowGameScreen(new CreateGame());
            } else
            {
                NextManagerOrContinueDay();
            }

        }

        public void CreateBackground()
        {
            Canvas c = new Canvas();
            Panel.SetZIndex(c, -9999);
            Image i = new Image();
            i.Stretch = Stretch.Fill;
            i.Source = ImageResources.GetImage(ImageResourceList.Background);
            i.Width = this.Width;
            i.Height = this.Height;
            Grid.SetColumn(c, 0);
            Grid.SetRow(c, 2);
            Grid.SetColumnSpan(c, 6);
            c.Children.Add(i);

            grdMain.Children.Add(c);
        }

        private void ShowGameScreen(IGameScreen NewGameScreen)
        {
            ShowGameScreen(NewGameScreen, new GameScreenSetup());
        }

        private void ShowGameScreen(IGameScreen NewGameScreen, GameScreenSetup Data)
        {
            while(MainArea.Children.Count > 0)
            {
                MainArea.Children.RemoveAt(0);
            }

            NewGameScreen.SetupGameScreenData(Data);
            MainArea.Children.Add((UIElement)NewGameScreen);
            
            CurrentScreen = NewGameScreen;
            Headings();
            CreateButtons();
        }

        private void Headings()
        {
            Title1.Text = CurrentScreen.SetupData.Title1;
            Title2.Text = CurrentScreen.SetupData.Title2;

            if(CurrentScreen.SetupData.ShowDate)
            {
                WorldAdapter wa = new WorldAdapter();

                GameDate.Visibility = Visibility.Visible;
                GameDate.Text = wa.CurrentDate.ToString("dd MMMM yyyy");
            } else
            {
                GameDate.Visibility = Visibility.Hidden;
            }
        }

        private void CreateButtons()
        {
            MainButton0.Visibility = Visibility.Hidden;
            MainButton1.Visibility = Visibility.Hidden;
            MainButton2.Visibility = Visibility.Hidden;
            MainButton3.Visibility = Visibility.Hidden;

            List<String> ButtonText = CurrentScreen.SetupData.MainButtons;

            int b = 0; // Starting Button 0, we'll populate in reverse

            for(int i = ButtonText.Count - 1; i >=0; i--)
            {
                Button but = (Button)this.FindName("MainButton" + b.ToString().Trim());
                if(but == null)
                {
                    throw new Exception("Unable to find MainButton" + b.ToString());
                }

                but.Visibility = Visibility.Visible;
                but.Content = ButtonText[i];

                b++;
            }

            ContinueButton.Visibility = (CurrentScreen.SetupData.ShowContinueButton ? Visibility.Visible : Visibility.Hidden);
        }

        private void MainButton_Click(object sender, RoutedEventArgs e)
        {
            Button b = (Button)sender;
            
            int id = Convert.ToInt32(b.Name.Substring(10));
            ScreenReturnData result = CurrentScreen.MainButtonClick(id);

            if(result == null)
            {
                return;
            }

            switch(result.ReturnCode)
            {
                case ScreenReturnCode.Ok:
                case ScreenReturnCode.Cancel:
                    // Special Case for new game...
                    if (ShowingNewGame)
                    {
                        ShowingNewGame = false;
                        NextManagerOrContinueDay(); // Start manager loop
                    } else
                    {
                        // TODO: Go back to prev screen
                        MessageBox.Show("TODO: Go back to prev screen");
                    }
                    break;

                case ScreenReturnCode.Error:
                    MessageBox.Show(result.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    break;

                case ScreenReturnCode.MatchdayComplete:
                    RunEndOfDayAndGoToNextDay();
                    break;

                default:
                    throw new NotImplementedException();
            }

            
           
        }

        private void ContinueButton_Click(object sender, RoutedEventArgs e)
        {
            NextManagerOrContinueDay();
        }

        private void NextManagerOrContinueDay()
        {
            ManagerAdapter ma = new ManagerAdapter();

            if (HumanManagers == null)
            {
                // Going to first manager, which means we have to run Start Of Day, unless loading from a savegame
                ProcessManager.RunStartOfDay(SaveGameJustLoaded);
                SaveGameJustLoaded = false;

                HumanManagers = ma.GetHumanManagers();
                PlayingHumanManager = 0;

                if(HumanManagers.Count() < 1)
                {
                    throw new Exception("No human managers");
                }
            } else
            {
                PlayingHumanManager++;
                if(PlayingHumanManager >= HumanManagers.Count())
                {
                    HumanManagers = null;
                }
            }

            if(HumanManagers != null)
            {
                // Display home screen for the current manager
                GameScreenSetup data = new GameScreenSetup();
                data.ManagerData = HumanManagers[PlayingHumanManager];
                ShowGameScreen(new Home(), data);
            } else
            {
                FixtureAdapter fa = new FixtureAdapter();

                if (fa.IsTodayAMatchDay())
                {
                    ShowGameScreen(new MatchdayMain());
                } else
                {
                    RunEndOfDayAndGoToNextDay();
                }
            }
        }

        private void RunEndOfDayAndGoToNextDay()
        {
            ProcessManager.RunEndOfDay();

            // Goto Next day
            WorldAdapter wa = new WorldAdapter();
            GameDate.Text = wa.CurrentDate.ToString("dd MMMM yyyy");
            NextManagerOrContinueDay();
        }

        private void imgLogo_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if(!e.Handled)
            {
                grdPopup.Visibility = (grdPopup.Visibility == Visibility.Visible ? Visibility.Hidden : Visibility.Visible);
                e.Handled = true;
            }
        }

        private void PopupButton_Click(object sender, RoutedEventArgs e)
        {
            grdPopup.Visibility = Visibility.Hidden;

            switch ((sender as Button).Name.ToUpper())
            {
                case "SAVEBUTTON":
                    SaveGame(null);
                    break;

                case "QUITBUTTON":
                    QuitGame();
                    break;

                default:
                    throw new Exception("Don't know what to do with this popup");
            }
            
        }

        private void SaveGame(string NewName)
        {
            WorldAdapter wa = new WorldAdapter();
            if(NewName != null && NewName != "")
            {
                wa.SaveGameName = NewName;
            }

            if (wa.SaveGameName == "" || wa.SaveGameName == null)
            {
                UiUtils.OpenTextInputDialog(grdMain, "Save Game", "This game has not been saved before, please give it a name.", SaveGameInputCallback);

            } 
            else
            {
                GameIO io = new GameIO();
                io.SaveGameName = wa.SaveGameName;
                io.SaveGame();

                List<DialogButton> buttons = new List<DialogButton>();
                buttons.Add(new DialogButton("OK", null, null));

                UiUtils.OpenDialogBox(grdMain, "Save Game", String.Format("Game saved successfully as \"{0}\"", wa.SaveGameName), buttons);
            }

            
        }

        private void SaveGameInputCallback(object Data)
        {
            if(Data != null)
            {
                SaveGame((Data as TextBox).Text);
            }
        }

        private void QuitGame()
        {
            List<DialogButton> buttons = new List<DialogButton>();

            buttons.Add(new DialogButton("Yes", QuitGameCallback, null));
            buttons.Add(new DialogButton("No", null, null));

            UiUtils.OpenDialogBox(grdMain, "Quit Game", "Are you sure you want to quit and lose any unsaved progress?", buttons);
        }

        private void QuitGameCallback(object Data)
        {
            Application.Current.Shutdown();
        }

        private void Window_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if(grdPopup.Visibility == Visibility.Visible && !e.Handled)
            {
                if(CheckObjectIsThePopup(e.Source) == false)
                {
                    grdPopup.Visibility = Visibility.Hidden;
                }
            }
        }

        private bool CheckObjectIsThePopup(object src)
        {
            if((src as Button) != null)
            {
                if ((src as Button).Name.ToUpper() == "SAVEBUTTON" || (src as Button).Name.ToUpper() == "QUITBUTTON")
                {
                    return true;
                }
            } 

            return false;
            
        }
    }
}
