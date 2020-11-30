﻿using System;
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
            const int WIDTH = 20;
            const int HEIGHT = 19;

            StackPanel sp = new StackPanel();
            sp.Orientation = Orientation.Horizontal;

            int Whole = (int)Math.Floor(Stars);
            bool firstStar = false;

            for(int i = 1; i <= Whole; i++)
            {
                Image Star = new Image();
                Star.Source = ImageResources.GetImage(ImageResourceList.Star_Whole);
                Star.Width = WIDTH;
                Star.Height = HEIGHT;

                if(!firstStar)
                {
                    Star.Margin = new Thickness(3, 0, 0, 0);
                }
                sp.Children.Add(Star);

                firstStar = true;
            }

            if(Stars - Whole > 0.5)
            {
                Image Star = new Image();
                Star.Source = ImageResources.GetImage(ImageResourceList.Star_Half);
                Star.Width = WIDTH;
                Star.Height = HEIGHT;

                if (!firstStar)
                {
                    Star.Margin = new Thickness(3, 0, 0, 0);
                }
                sp.Children.Add(Star);
            }

            return sp;
        }
    }
}
