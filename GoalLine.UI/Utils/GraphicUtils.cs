using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using GoalLine.Resources;
using System.Windows;
using System.Windows.Shapes;
using System.Windows.Media;

namespace GoalLine.UI.Utils
{
    static class GraphicUtils
    {

        const int STARWIDTH = 20;
        const int STARHEIGHT = 19;

        /// <summary>
        /// Returns an image of a football Shirt in a Polygon object
        /// </summary>
        /// <returns></returns>
        public static Polygon Shirt()
        {
            Polygon p = new Polygon();
            p.Stroke = Brushes.Black;
            p.Fill = Brushes.BlueViolet;
            p.StrokeThickness = 1;
            p.HorizontalAlignment = HorizontalAlignment.Left;
            p.VerticalAlignment = VerticalAlignment.Center;
            p.Points = new PointCollection() { new Point(8, 0),
                new Point(42, 0),
                new Point(50, 12.5),
                new Point(43.75, 25),
                new Point(37.5, 12.5),
                new Point(37.5, 50),
                new Point(12.5, 50),
                new Point(12.5, 12.5),
                new Point(6.25, 25),
                new Point(0, 12.5),
                new Point(8, 0)
            };

            return p;
        }

        /// <summary>
        /// Returns a StackPanel containing the appropriate star images for a star rating
        /// </summary>
        /// <param name="Stars">Number of stars (e.g. 3.5)</param>
        /// <returns></returns>
        public static StackPanel StarRating(double Stars)
        {
            

            StackPanel sp = new StackPanel();
            sp.Orientation = Orientation.Horizontal;

            int Whole = (int)Math.Floor(Stars);
            bool firstStar = false;

            for(int i = 1; i <= Whole; i++)
            {
                Image Star = StarImage(true);

                if(!firstStar)
                {
                    Star.Margin = new Thickness(3, 0, 0, 0);
                }
                sp.Children.Add(Star);

                firstStar = true;
            }

            if(Stars - Whole >= 0.45)
            {
                Image Star = StarImage(false);

                if (!firstStar)
                {
                    Star.Margin = new Thickness(3, 0, 0, 0);
                }
                sp.Children.Add(Star);
            }

            return sp;
        }

        public static StackPanel StarRatingWithNumber(double Stars)
        {
            StackPanel sp = new StackPanel();
            sp.Orientation = Orientation.Horizontal;

            Image star = StarImage(true);
            star.Width -= 4;
            star.Height -= 4;
            sp.Children.Add(star);

            TextBlock t = new TextBlock();
            t.Text = Stars.ToString("0.0");
            t.Margin = new Thickness(3, 0, 0, 0);
            t.Style = Application.Current.FindResource("ListHeader") as Style;
            t.FontSize = 16;
            
            sp.Children.Add(t);


            return sp;
        }

        private static Image StarImage(bool Whole)
        {
            Image Star = new Image();
            Star.Source = ImageResources.GetImage(Whole ? ImageResourceList.Star_Whole : ImageResourceList.Star_Half);
            Star.Width = STARWIDTH;
            Star.Height = STARHEIGHT;

            return Star;
        }
    }
}
