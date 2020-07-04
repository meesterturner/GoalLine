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

namespace GoalLine.UI.SingleControls
{
    /// <summary>
    /// Interaction logic for PagingControl.xaml
    /// </summary>
    public partial class PagingControl : UserControl
    {
        public event EventHandler GoBackClicked;
        public event EventHandler GoForwardClicked;
        public event EventHandler EitherDirectionClicked;

        private int _CurrentItem = -1;
        public int CurrentItem
        {
            get
            {
                return _CurrentItem;
            }

            set
            {
                DisplayItem(value);
            }
        }
        public List<PagingItem> Items = new List<PagingItem>();

        public void DisplayItem(int Sequence)
        {
            _CurrentItem = Sequence;
            lblCaption.Text = Items[CurrentItem].Name;
        }

        public PagingControl()
        {
            InitializeComponent();
        }

        private void GoForward_Click(object sender, RoutedEventArgs e)
        {
            _CurrentItem++;
            if(CurrentItem >= Items.Count)
            {
                _CurrentItem = 0;
            }

            DisplayItem(CurrentItem);
            GoForwardClicked?.Invoke(this, EventArgs.Empty);
            EitherDirectionClicked?.Invoke(this, EventArgs.Empty);
        }

        private void GoBack_Click(object sender, RoutedEventArgs e)
        {
            _CurrentItem--;
            if (CurrentItem < 0)
            {
                _CurrentItem = Items.Count - 1;
            }

            DisplayItem(CurrentItem);
            GoBackClicked?.Invoke(this, EventArgs.Empty);
            EitherDirectionClicked?.Invoke(this, EventArgs.Empty);
        }
    }

    public class PagingItem
    {
        public int ID { get; set; }
        public string Name { get; set; }
    }
}
