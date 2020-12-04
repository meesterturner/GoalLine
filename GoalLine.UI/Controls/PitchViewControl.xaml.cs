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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using GoalLine.Resources;

namespace GoalLine.UI.Controls
{
    /// <summary>
    /// Interaction logic for PitchViewControl.xaml
    /// </summary>
    public partial class PitchViewControl : UserControl
    {

        public double MinX { get; set; }
        public double MaxX { get; set; }
        public double MinY { get; set; }
        public double MaxY { get; set; }
        public bool Animate { get; set; } = false;
        public int AnimateMillisecs { get; set; } = 300;

        private (double x, double y) _BallPosition;
        public (double x, double y) BallPosition
        {
            get
            {
                return _BallPosition;
            }

            set
            {
                double TempX = Canvas.GetLeft(BallImage);
                double Tempy = Canvas.GetTop(BallImage);

                _BallPosition = value;
                if(Animate)
                {
                    AnimateBall();
                } 
                else
                {
                    MoveBall();
                }
                
            }
        }

        private const int BALLW = 30;
        private const int BALLH = 30;


        public PitchViewControl()
        {
            InitializeComponent();

            BallImage.Source = ImageResources.GetImage(ImageResourceList.SmallBall);
            BallImage.Width = BALLW;
            BallImage.Height = BALLH;
            BallImage.Visibility = Visibility.Hidden;

            MinX = -2;
            MaxX = 2;
            MinY = 0;
            MaxY = 2;
        }

        private void MoveBall()
        {
            (double x, double y) Pos = CalculatePixelPosition();
            BallImage.Visibility = Visibility.Visible;

            Canvas.SetLeft(BallImage, Pos.x);
            Canvas.SetTop(BallImage, Pos.y);
        }

        private void AnimateBall()
        {
            (double x, double y) Pos = CalculatePixelPosition();
            BallImage.Visibility = Visibility.Visible;

            DoubleAnimation xAnim = new DoubleAnimation();
            xAnim.From = Canvas.GetLeft(BallImage);
            xAnim.To = Pos.x;
            xAnim.Duration = new Duration(TimeSpan.FromMilliseconds(AnimateMillisecs));

            DoubleAnimation yAnim = new DoubleAnimation();
            yAnim.From = Canvas.GetTop(BallImage);
            yAnim.To = Pos.y;
            yAnim.Duration = new Duration(TimeSpan.FromMilliseconds(AnimateMillisecs));

            TranslateTransform tx = new TranslateTransform();
            BallImage.RenderTransform = tx;

            tx.BeginAnimation(TranslateTransform.XProperty, xAnim);
            tx.BeginAnimation(TranslateTransform.YProperty, yAnim);
        }

        private (double x, double y) CalculatePixelPosition()
        {
            double SegXCount = (MaxX - MinX) + 1;
            double SegW = PitchCanvas.Width / SegXCount;
            double SegX = BallPosition.x - MinX;

            double PixelX = (SegX * SegW) + ((SegW / 2) - (BALLW / 2));


            double SegYCount = (MaxY - MinY) + 1;
            double SegH = PitchCanvas.Height / SegYCount;
            double SegY = BallPosition.y - MinY;

            double PixelY = (SegY * SegH) + ((SegH / 2) - (BALLH / 2));

            return (PixelX, PixelY);
        }
    }
}
