using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using GoalLine.Data;
using GoalLine.Structures;
using GoalLine.Processes;
using GoalLine.UI.GameScreens;

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
        }

        public void StartGame(bool SaveGameLoaded)
        {
            SaveGameJustLoaded = SaveGameLoaded;

            if (!SaveGameLoaded)
            {
                ShowingNewGame = true;
                ShowGameScreen(new CreateGame());
            } else
            {
                NextManagerOrContinueDay();
            }

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
                        MessageBox.Show("TODO: Go back to prev screen");
                    }
                    break;

                case ScreenReturnCode.Error:
                    MessageBox.Show(result.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Exclamation);
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
                MessageBox.Show("Go to matches");
            }
        }
    }
}
