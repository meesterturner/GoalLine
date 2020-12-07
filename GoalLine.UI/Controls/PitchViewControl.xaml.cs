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

        public bool Animate { get; set; } = false;
        public int AnimateMillisecs { get; set; } = 300;

        private (double x, double y) _BallPosition;
        private (double x, double y) _LastBallPixelPos;
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

                if (Animate)
                {
                    AnimateBall(); // AnimateBall();
                } 
                else
                {
                    MoveBall();
                }
                
            }
        }

        private const int BALLW = 30;
        private const int BALLH = 30;
        public int SegmentCountX => 8;
        public int SegmentCountY => 5;

        public PitchViewControl()
        {
            InitializeComponent();

            PitchImage.Source = ImageResources.GetImage(ImageResourceList.PitchSmallLandscape);
            PitchImage.Stretch = Stretch.Fill;
            BallImage.Source = ImageResources.GetImage(ImageResourceList.SmallBall);
            BallImage.Width = BALLW;
            BallImage.Height = BALLH;

            CentreBall();
        }

        public void CentreBall()
        {
            bool animTemp = Animate;
            Animate = false;
            BallPosition = (((double)SegmentCountX - 1) / 2, ((double)SegmentCountY - 1) / 2);
            Animate = animTemp;
        }

        private void MoveBall()
        {
            (double x, double y) Pos = CalculatePixelPosition();

            Canvas.SetLeft(BallImage, Pos.x);
            Canvas.SetTop(BallImage, Pos.y);

            _LastBallPixelPos = Pos;
        }

        private void AnimateBall()
        {

            (double x, double y) Pos = CalculatePixelPosition();

            DoubleAnimation xAnim = new DoubleAnimation();
            xAnim.By = Pos.x - _LastBallPixelPos.x;
            xAnim.Duration = new Duration(TimeSpan.FromMilliseconds(AnimateMillisecs));
            xAnim.DecelerationRatio = 0.3;

            DoubleAnimation yAnim = new DoubleAnimation();
            yAnim.By = Pos.y - _LastBallPixelPos.y;
            yAnim.Duration = new Duration(TimeSpan.FromMilliseconds(AnimateMillisecs));
            yAnim.DecelerationRatio = 0.3;

            TranslateTransform tx = new TranslateTransform();
            BallImage.RenderTransform = tx;
            
            tx.BeginAnimation(TranslateTransform.XProperty, xAnim);
            tx.BeginAnimation(TranslateTransform.YProperty, yAnim);
            
            Canvas.SetLeft(BallImage, _LastBallPixelPos.x);
            Canvas.SetTop(BallImage, _LastBallPixelPos.y);
            _LastBallPixelPos = Pos;
        }

        private (double x, double y) CalculatePixelPosition()
        {
            double SegW = PitchCanvas.Width / SegmentCountX;
            double PixelX = (BallPosition.x * SegW) + ((SegW - BALLW) / 2);

            double SegH = PitchCanvas.Height / SegmentCountY;
            double PixelY = (BallPosition.y * SegH) + ((SegH - BALLH) / 2);

            return (PixelX, PixelY);
        }
    }
}
