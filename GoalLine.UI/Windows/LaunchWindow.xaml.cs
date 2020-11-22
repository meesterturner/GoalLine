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

namespace GoalLine.UI
{
    /// <summary>
    /// Interaction logic for LaunchWindow.xaml
    /// </summary>
    public partial class LaunchWindow : Window
    {
        public LaunchWindow()
        {
            InitializeComponent();
        }

        private void cmdStart_Click(object sender, RoutedEventArgs e)
        {
            Initialiser init = new Initialiser();

            init.CreateWorld();

            GameWindow g = new GameWindow();
            g.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            g.WindowState = WindowState.Maximized;
            g.Show();
            g.StartGame(false);

            this.Close();
        }
    }
}
