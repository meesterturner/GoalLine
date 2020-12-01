using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Shapes;
using System.Windows.Media;
using System.Windows;
using GoalLine.Resources;
using System.Windows.Media.Imaging;

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

        public static Dictionary<string, object> PleaseWait(Grid ExistingGrid, string Text)
        {
            Grid Overlay = DarkOverlayGrid(ExistingGrid);

            StackPanel sp = new StackPanel();
            sp.HorizontalAlignment = HorizontalAlignment.Center;
            sp.VerticalAlignment = VerticalAlignment.Center;

            ProgressBar pb = new ProgressBar();
            pb.Width = 150;
            pb.Height = 15;
            pb.IsIndeterminate = true;

            sp.Children.Add(pb);

            TextBlock tb = QuickTextblock(Text, false);

            sp.Children.Add(tb);

            Overlay.Children.Add(sp);

            Dictionary<string, object> obj = new Dictionary<string, object>();
            obj.Add(ReturnedUIObjects.GridContainer, Overlay);
            obj.Add(ReturnedUIObjects.Caption, tb);
            obj.Add(ReturnedUIObjects.ProgressBar, pb);
            return obj;
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

        private static void RemoveControl(UIElement child)
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
