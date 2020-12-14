using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Controls;
using System.Windows.Shapes;
using System.Windows.Media;
using System.Windows;
using GoalLine.Resources;
using System.Windows.Media.Imaging;
using System.Windows.Input;
using System.Windows.Threading;
using GoalLine.UI.Controls;

namespace GoalLine.UI.Utils
{
    public static class ReturnedUIObjects
    {
        public const string GridContainer = "GRID";
        public const string Caption = "CAPTION";
        public const string ProgressBar = "PROGRESS";
    }

    public class DialogButton
    {
        public string Text { get; set; }
        public DialogButtonCallback Callback { get; set; }
        public object CallbackData { get; set; }

        public DialogButton()
        {

        }

        public DialogButton(string Text, DialogButtonCallback Callback, object CallbackData)
        {
            this.Text = Text;
            this.Callback = Callback;
            this.CallbackData = CallbackData;
        }
    }

    public delegate void DialogButtonCallback(object Data);

    static class UiUtils
    {

        public static Grid MainWindowGrid { get; set; }

        public static void OpenTextInputDialog(Grid ExistingGrid, string Title, string Body, DialogButtonCallback Callback)
        {
            Grid Overlay = DarkOverlayGrid(ExistingGrid);

            StackPanel sp = new StackPanel();
            sp.HorizontalAlignment = HorizontalAlignment.Center;
            sp.VerticalAlignment = VerticalAlignment.Center;

            QuickAddTitleAndBody(sp, Title, Body);

            TextBox ti = new TextBox();
            ti.HorizontalAlignment = HorizontalAlignment.Center;
            ti.VerticalAlignment = VerticalAlignment.Center;
            ti.TextAlignment = TextAlignment.Center;
            sp.Children.Add(ti);

            sp.Children.Add(QuickTextblock(" ", false));

            List<DialogButton> buttons = new List<DialogButton>();
            buttons.Add(new DialogButton(LangResources.CurLang.OK, Callback, ti));
            buttons.Add(new DialogButton(LangResources.CurLang.Cancel, Callback, null));

            sp.Children.Add(DialogButtonGrid(Overlay, buttons));

            Overlay.Children.Add(sp);
            ti.Focus();
        }

        public static void OpenDialogBox(Grid ExistingGrid, string Title, string Body, List<DialogButton> Buttons)
        {
            Grid Overlay = DarkOverlayGrid(ExistingGrid);

            StackPanel sp = new StackPanel();
            sp.HorizontalAlignment = HorizontalAlignment.Center;
            sp.VerticalAlignment = VerticalAlignment.Center;

            QuickAddTitleAndBody(sp, Title, Body);

            sp.Children.Add(DialogButtonGrid(Overlay, Buttons));
            Overlay.Children.Add(sp);
        }


        public static Grid DarkOverlayGrid(Grid ExistingGrid)
        {
            return DarkOverlayGrid(ExistingGrid, 0.8);
        }

        public static Grid DarkOverlayGrid(Grid ExistingGrid, double Opacity)
        {
            Grid newGrid = new Grid();
            newGrid.Height = ExistingGrid.ActualHeight;
            newGrid.Width = ExistingGrid.ActualWidth;
            
            Rectangle myRect = new Rectangle();
            myRect.Stroke = Brushes.Black;
            myRect.Fill = Brushes.Black;
            myRect.Opacity = Opacity;
            myRect.HorizontalAlignment = HorizontalAlignment.Left;
            myRect.VerticalAlignment = VerticalAlignment.Center;
            myRect.Height = ExistingGrid.ActualHeight;
            myRect.Width = ExistingGrid.ActualWidth;

            newGrid.Children.Add(myRect);

            if (ExistingGrid.ColumnDefinitions.Count > 0)
            {
                Grid.SetColumnSpan(newGrid, ExistingGrid.ColumnDefinitions.Count);
            }
                
            if (ExistingGrid.RowDefinitions.Count > 0)
            {
                Grid.SetRowSpan(newGrid, ExistingGrid.RowDefinitions.Count);
            }
                
            Grid.SetColumn(newGrid, 0);
            Grid.SetRow(newGrid, 0);

            ExistingGrid.Children.Add(newGrid);

            return newGrid;
        }

        public static Dictionary<string, object> OpenPleaseWait(Grid ExistingGrid, string Text)
        {
            Grid Overlay = DarkOverlayGrid(ExistingGrid);
            Overlay.Cursor = Cursors.Wait;
            StackPanel sp = new StackPanel();
            sp.HorizontalAlignment = HorizontalAlignment.Center;
            sp.VerticalAlignment = VerticalAlignment.Center;

            PercentageBarControl pb = new PercentageBarControl();
            pb.Width = 602;
            pb.Height = 52;
            pb.Text = Text;

            sp.Children.Add(pb);

            Overlay.Children.Add(sp);

            Dictionary<string, object> obj = new Dictionary<string, object>();
            obj.Add(ReturnedUIObjects.GridContainer, Overlay);
            obj.Add(ReturnedUIObjects.ProgressBar, pb);
            return obj;
        }

        public static void AddGridData(Grid g, int x, int y, string caption, int data)
        {
            AddGridData_Worker(g, x, y, caption, data.ToString(), 2, true);
        }

        public static void AddGridData(Grid g, int x, int y, string caption, string data)
        {
            AddGridData_Worker(g, x, y, caption, data, 2, false);
        }

        public static void AddGridData_DoubleSize(Grid g, int x, int y, string caption, string data)
        {
            AddGridData_Worker(g, x, y, caption, data, 4, false);
        }

        private static void AddGridData_Worker(Grid g, int x, int y, string caption, string data, int span, bool redGreenBackground)
        {
            if(redGreenBackground)
            {
                const int LOWER = 30;
                const int HIGHER = 70;
                int number = Convert.ToInt32(data);

                if(number <= LOWER || number >= HIGHER)
                {
                    Rectangle r = new Rectangle();
                    r.Opacity = 0.2;
                    r.Width = (100 * span) - 2;
                    r.Height = 25;

                    r.Fill = (number <= LOWER ? Brushes.Red : Brushes.Green);
                    Grid.SetColumn(r, x);
                    Grid.SetRow(r, y);
                    Grid.SetColumnSpan(r, span);
                    g.Children.Add(r);
                }
            }

            TextBlock t;

            t = new TextBlock();
            t.Text = caption + ":";
            t.Style = Application.Current.FindResource("DialogTitleSmaller") as Style;
            t.Margin = new Thickness(8, 0, 0, 0);
            Grid.SetColumn(t, x);
            Grid.SetRow(t, y);
            Grid.SetColumnSpan(t, span);
            g.Children.Add(t);

            t = new TextBlock();
            t.Text = data;
            t.HorizontalAlignment = HorizontalAlignment.Right;
            t.Style = Application.Current.FindResource("ListHeader") as Style;
            t.Margin = new Thickness(0, 0, 8, 0);
            Grid.SetColumn(t, x);
            Grid.SetColumnSpan(t, span);
            Grid.SetRow(t, y);
            g.Children.Add(t);
        }

        public static void WaitForThread(Thread t)
        {
            while (t.IsAlive)
            {
                Application.Current.Dispatcher.Invoke(DispatcherPriority.Background, new Action(delegate { })); // DoEvents, kinda
            }
        }

        // ----- Helper functions -----
        #region Helper Functions
        private static void ProcessDialogCallback(Grid OverlayToRemove, DialogButtonCallback Callback, object CallbackData)
        {
            RemoveControl(OverlayToRemove);
            if(Callback != null)
            {
                Callback(CallbackData);
            }
        }

        public static void RemoveControl(UIElement child)
        {
            var parent = VisualTreeHelper.GetParent(child);
            var parentAsPanel = parent as Panel;
            if (parentAsPanel != null)
            {
                parentAsPanel.Children.Remove(child);
            }
            var parentAsContentControl = parent as ContentControl;
            if (parentAsContentControl != null)
            {
                parentAsContentControl.Content = null;
            }
            var parentAsDecorator = parent as Decorator;
            if (parentAsDecorator != null)
            {
                parentAsDecorator.Child = null;
            }
        }

        private static TextBlock QuickTextblock(string Text, bool IsTitle)
        {
            string Style = (IsTitle ? "DialogTitle" : "DialogBody");
            TextBlock tb = new TextBlock();
            tb.Text = Text;
            tb.Style = Application.Current.FindResource(Style) as Style;
            tb.TextWrapping = TextWrapping.Wrap;
            tb.MaxWidth = 700;
            tb.VerticalAlignment = VerticalAlignment.Center;

            return tb;
        }

        private static Grid DialogButtonGrid(Grid Overlay, List<DialogButton> buttons)
        {
            Grid buttonContainer = new Grid();
            for(int i = 0; i < buttons.Count(); i++)
            {
                buttonContainer.ColumnDefinitions.Add(new ColumnDefinition());
                DialogButton db = buttons[i];

                Button b = new Button();
                b.Content = buttons[i].Text;
                b.HorizontalAlignment = HorizontalAlignment.Center;
                b.VerticalAlignment = VerticalAlignment.Center;
                b.Click += new RoutedEventHandler((object sender, RoutedEventArgs e) => ProcessDialogCallback(Overlay, db.Callback, db.CallbackData));

                Grid.SetColumn(b, i);
                buttonContainer.Children.Add(b);
            }

            return buttonContainer;
        }

        private static void QuickAddTitleAndBody(StackPanel sp, string Title, string Body)
        {
            sp.Children.Add(QuickTextblock(Title, true));
            sp.Children.Add(QuickTextblock(" ", false));
            sp.Children.Add(QuickTextblock(Body, false));
            sp.Children.Add(QuickTextblock(" ", false));
        }

        #endregion
    }
}
