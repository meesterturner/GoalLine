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

        public GameWindow()
        {
            InitializeComponent();
            ShowGameScreen();
        }

        private void ShowGameScreen()
        {
            while(MainArea.Children.Count > 0)
            {
                MainArea.Children.RemoveAt(0);
            }

            IGameScreen NewGameScreen;

            NewGameScreen = new CreateGame();
            NewGameScreen.SetupGameScreenData(new GameScreenSetup());
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
                    MessageBox.Show("Got to do the OK shizzle here!");
                    break;

                case ScreenReturnCode.Error:
                    MessageBox.Show(result.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    break;

                default:
                    throw new NotImplementedException();
            }
           
        }
    }
}
