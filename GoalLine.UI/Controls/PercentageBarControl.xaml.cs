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

namespace GoalLine.UI.Controls
{
    /// <summary>
    /// Interaction logic for PercentageBarControl.xaml
    /// </summary>
    public partial class PercentageBarControl : UserControl
    {
        private int _Percentage = 100;
        private int _Width;
        private double _WidthMultiplier;
        public int Percentage {
            get
            {
                return _Percentage;
            }

            set
            {
                _Percentage = value;
                Redraw();
            }
        }

        private string _Text;
        public string Text
        {
            get
            {
                return _Text;
            }

            set
            {
                _Text = value;
                Caption.Text = _Text;
            }
        }

        private void Redraw()
        {
            InnerRect.Width = Percentage * _WidthMultiplier;
            MiddleFade.CenterX = Percentage;
        }


        public PercentageBarControl()
        {
            InitializeComponent();
            _Width = Convert.ToInt32(InnerRect.Width);
            _WidthMultiplier = _Width / 100;
            
            Text = "";
        }
    }
}
