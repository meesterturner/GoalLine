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
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Threading;

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
            SetupText();
            grdPopup.Visibility = Visibility.Hidden;
            imgLogo.Source = ImageResources.GetImage(ImageResourceList.LogoSmall);

        }

        private void SetupText()
        {
            SaveButton.Content = LangResources.CurLang.SaveGame;
            QuitButton.Content = LangResources.CurLang.QuitGame;
            ContinueButton.Content = LangResources.CurLang.Next + " >";

            HomeButton.Content = LangResources.CurLang.Home;
            LeagueButton.Content = LangResources.CurLang.League;
            TeamButton.Content = LangResources.CurLang.Team;
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
                    ShowErrorDialog(result.Message);
                    break;

                case ScreenReturnCode.MatchdayComplete:
                    RunProcesses(true);
                    break;

                default:
                    throw new NotImplementedException();
            }

            
           
        }

        private void ShowErrorDialog(string Message)
        {
            UiUtils.OpenDialogBox(UiUtils.MainWindowGrid, LangResources.CurLang.Error, Message, new List<DialogButton>() { new DialogButton(LangResources.CurLang.OK, null, null) });
        }

        private void ContinueButton_Click(object sender, RoutedEventArgs e)
        {
            ScreenReturnData result;

            try
            {
                result = CurrentScreen.ContinueButtonClick();
            }
            catch (NotImplementedException)
            {
                result = new ScreenReturnData(ScreenReturnCode.None);
            }


            switch (result.ReturnCode)
            {
                case ScreenReturnCode.Ok:
                case ScreenReturnCode.None:
                    NextManagerOrContinueDay();
                    break;

                case ScreenReturnCode.Error:
                    ShowErrorDialog(result.Message);
                    break;

                default:
                    throw new NotImplementedException("This return code not implemented for Continue button");
            }  
        }

        private void NextManagerOrContinueDay()
        {
            ManagerAdapter ma = new ManagerAdapter();
            FixtureAdapter fa = new FixtureAdapter();
            WorldAdapter wa = new WorldAdapter();
            TeamAdapter ta = new TeamAdapter();

            if (HumanManagers == null)
            {
                // Going to first manager, which means we have to run Start Of Day, unless loading from a savegame
                if(!SaveGameJustLoaded)
                {
                    RunProcesses(false);
                } 
                else
                {
                    SaveGameJustLoaded = false;
                }
                
                

                HumanManagers = ma.GetHumanManagers();
                PlayingHumanManager = 0;

                if(HumanManagers.Count() < 1)
                {
                    throw new Exception("No human managers");
                }
            } else
            {
                // Check selections if a matchday
                if(fa.IsTodayAMatchDay(HumanManagers[PlayingHumanManager].CurrentTeam))
                {
                    const int REQUIREDCOUNT = 11;
                    int PlayerCount = ta.CountSelectedPlayers(HumanManagers[PlayingHumanManager].CurrentTeam);
                    if(PlayerCount < REQUIREDCOUNT)
                    {
                        UiUtils.OpenDialogBox(UiUtils.MainWindowGrid, LangResources.CurLang.MatchDay,
                            string.Format(LangResources.CurLang.YouHaveNotSelectedEnoughPlayers, PlayerCount, REQUIREDCOUNT),
                            new List<DialogButton>() { new DialogButton(LangResources.CurLang.OK, null, null) });

                        return;
                    }
                }

                // Go to next human manager
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

                if(fa.IsTodayAMatchDay(data.ManagerData.CurrentTeam))
                {
                    Fixture f = fa.GetNextFixture(data.ManagerData.CurrentTeam, wa.CurrentDate);
                    int Opposition = (f.TeamIDs[0] != data.ManagerData.CurrentTeam ? f.TeamIDs[0] : f.TeamIDs[1]);
                    string Message = string.Format(LangResources.CurLang.YouHaveAMatchAgainst, ta.GetTeam(Opposition).Name);

                    UiUtils.OpenDialogBox(UiUtils.MainWindowGrid, LangResources.CurLang.MatchDay, Message,
                        new List<DialogButton>() { 
                            new DialogButton(LangResources.CurLang.OK, null, null) 
                        });
                }
            } else
            {
                
                if (fa.IsTodayAMatchDay())
                {
                    ShowGameScreen(new MatchdayMain());
                } else
                {
                    RunProcesses(true);
                }
            }
        }

        private void RunProcesses(bool EndOfDay)
        {
            Dictionary<string, object> WaitScreen = UiUtils.OpenPleaseWait(UiUtils.MainWindowGrid, LangResources.CurLang.DailyUpdate);

            Thread t = new Thread(() =>
            {
                Thread.CurrentThread.IsBackground = true;
                Thread.Sleep(250);

                if (EndOfDay)
                {
                    ProcessManager.RunEndOfDay(); 
                }
                else
                {
                    ProcessManager.RunStartOfDay(SaveGameJustLoaded);
                }
            });

            t.Start();

            while(t.IsAlive)
            {
                Application.Current.Dispatcher.Invoke(DispatcherPriority.Background, new Action(delegate { })); // DoEvents, kinda
            }

            UiUtils.RemoveControl((WaitScreen[ReturnedUIObjects.GridContainer] as Grid));

            if(EndOfDay)
            {
                // Goto Next day
                WorldAdapter wa = new WorldAdapter();
                GameDate.Text = wa.CurrentDate.ToString("dd MMMM yyyy");
                NextManagerOrContinueDay();
            }
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
                UiUtils.OpenTextInputDialog(grdMain, LangResources.CurLang.SaveGame, LangResources.CurLang.GameHasNotBeenSavedBefore, SaveGameInputCallback);

            } 
            else
            {
                GameIO io = new GameIO();
                io.SaveGameName = wa.SaveGameName;
                io.SaveGame();

                List<DialogButton> buttons = new List<DialogButton>();
                buttons.Add(new DialogButton(LangResources.CurLang.OK, null, null));

                UiUtils.OpenDialogBox(grdMain, LangResources.CurLang.SaveGame, String.Format(LangResources.CurLang.GameSavedSuccessfully, wa.SaveGameName), buttons);
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

            buttons.Add(new DialogButton(LangResources.CurLang.Yes, QuitGameCallback, null));
            buttons.Add(new DialogButton(LangResources.CurLang.No, null, null));

            UiUtils.OpenDialogBox(grdMain, LangResources.CurLang.QuitGame, LangResources.CurLang.AreYouSureYouWantToQuit, buttons);
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
