using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using GoalLine.Resources;

namespace GoalLine.UI.Controls
{
    /// <summary>
    /// Interaction logic for ListControl.xaml
    /// </summary>
    /// 
    public delegate void ItemClickCallback();

    public enum SelectMode
    {
        Highlight = 0,
        Callback = 1,
        HighlightAndCallback = 2
    }

    public partial class ListControl : UserControl
    {
        private Brush DeselectedBackgroundBrush = Brushes.Transparent;
        private double DeselectedOpacity = 0.1;

        private Brush SelectedBackgroundBrush = Brushes.MediumPurple;
        private double SelectedOpacity = 0.8;

        public SelectMode SelectionMode { get; set; } = SelectMode.Highlight;

        public string Title { get; set; }
        public ItemClickCallback Callback_ItemClick { get; set; }
        public List<ListColumn> Columns { get; set; }

        private List<ListRow> _Rows;
        public List<ListRow> Rows { 
            get 
            {
                return _Rows;
            }
            
            set 
            {
                _Rows = value;
                Render();
            }
        }

        private int _SelectedID = -1;
        public int SelectedID
        {
            get
            {
                return _SelectedID;
            }

            private set
            {
                _SelectedID = value;
            }
        }

        private Grid grdHeader;
        private Grid grdRows;

        private Dictionary<int, Rectangle> RowBackgrounds;

        private void Render()
        {
            const bool GRIDLINES = false;
            const int ROWHEIGHT = 25;
            int x;
            int y;

            lblCaption.Text = Title;
            grdMain.ShowGridLines = GRIDLINES;
            grdMain.RowDefinitions[0].Height = new GridLength((Title == "" ? 0 : 30));

            // Initialise Grids
            if (grdHeader != null)
            {
                grdMain.Children.Remove((UIElement)this.FindName("grdHeader"));
                grdMain.Children.Remove((UIElement)this.FindName("grdRows"));
            }

            grdHeader = new Grid();
            grdHeader.Name = "grdHeader";

            grdRows = new Grid();
            grdRows.Name = "grdRows";

            grdHeader.Height = 30;
            grdHeader.ShowGridLines = GRIDLINES;

            grdRows.ShowGridLines = GRIDLINES;

            int TotWidth = 0;

            foreach (ListColumn c in Columns)
            {
                for(int i = 1; i <= 2; i++)
                {
                    ColumnDefinition h = new ColumnDefinition();
                    h.Width = new GridLength(c.Width);
                    h.MinWidth = c.Width;
                    h.MaxWidth = c.Width;

                    if(i == 1)
                    {
                        grdHeader.ColumnDefinitions.Add(h);
                        TotWidth += c.Width;
                    } else
                    {
                        grdRows.ColumnDefinitions.Add(h);
                    }
                }
            }

            // Render Column Headers
            x = 0;
            

            for(x = 0; x < Columns.Count(); x++)
            {
                TextBlock heading = new TextBlock();
                heading.Text = Columns[x].Title;
                heading.HorizontalAlignment = Columns[x].Alignment;
                heading.Style = Application.Current.FindResource("ListHeader") as Style;
                Grid.SetColumn(heading, x);
                grdHeader.Children.Add(heading);
            }

            Grid.SetRow(grdHeader, 1);
            grdMain.Children.Add(grdHeader);

            Line l = new Line();
            l.X1 = 0;
            l.X2 = TotWidth; // grdMain.ActualWidth;
            l.Y1 = 1;
            l.Y2 = 1;
            l.Margin = new Thickness(0, 0, 0, 4);
            l.Stroke = Application.Current.FindResource("StandardGrey_Brush") as Brush;
            l.StrokeThickness = 2;
            l.VerticalAlignment = VerticalAlignment.Bottom;
            Grid.SetColumnSpan(l, grdHeader.ColumnDefinitions.Count());
            Grid.SetRow(l, 1);
            grdMain.Children.Add(l);

            // Render rows
            y = 0;
            RowBackgrounds = new Dictionary<int, Rectangle>();

            if(Rows != null)
            {
                foreach (ListRow r in Rows)
                {
                    RowDefinition gridRowDef = new RowDefinition();
                    gridRowDef.Height = new GridLength(ROWHEIGHT);
                    grdRows.RowDefinitions.Add(gridRowDef);

                    x = 0;

                    // Background lowlight/highlight
                    Rectangle rect = new Rectangle();
                    rect.Stroke = DeselectedBackgroundBrush;
                    rect.Fill = DeselectedBackgroundBrush;
                    rect.Opacity = DeselectedOpacity;
                    rect.Height = 28;
                    rect.Width = 2000; // grdRows.ActualWidth;
                    rect.Margin = new Thickness(0, 0, 0, 2);

                    Grid.SetRow(rect, y);
                    Grid.SetColumn(rect, 0);
                    Grid.SetColumnSpan(rect, grdRows.ColumnDefinitions.Count());
                    grdRows.Children.Add(rect);

                    RowBackgrounds[r.ID] = rect;
                    rect = null;

                    foreach (object s in r.ColumnData)
                    {
                        UIElement cell;

                        if(s != null)
                        {
                          
                            if(s.GetType() == typeof(string))
                            {
                                TextBlock tb = new TextBlock();
                                tb.Text = s.ToString();
                                tb.HorizontalAlignment = Columns[x].Alignment;
                                tb.VerticalAlignment = VerticalAlignment.Center;
                                tb.Style = Application.Current.FindResource("ListItem") as Style;

                                cell = tb;
                            } 
                            else if(s.GetType() == typeof(StackPanel))
                            {
                                StackPanel sp = new StackPanel();
                                sp = (StackPanel)s;
                                sp.VerticalAlignment = VerticalAlignment.Center;
                                sp.HorizontalAlignment = Columns[x].Alignment;
                                cell = sp;
                            }

                            else
                            {
                                throw new NotImplementedException("Don't know what to do with this object in the grid");
                            }

                            Grid.SetRow(cell, y);
                            Grid.SetColumn(cell, x);
                            grdRows.Children.Add(cell);
                        }
                        x++;
                    }

                    // Foreground rectangle, transparently painted over each row
                    // used as the clickable item. This means we only have one event handler
                    // per row, rather than the background plus one per column in the row
                    rect = new Rectangle();
                    rect.Stroke = Brushes.Transparent;
                    rect.Fill = Brushes.Transparent;
                    rect.Height = RowBackgrounds[r.ID].Height;
                    rect.Width = RowBackgrounds[r.ID].Width;
                    rect.Margin = RowBackgrounds[r.ID].Margin;
                    rect.Cursor = Cursors.Hand;

                    rect.MouseLeftButtonUp += new MouseButtonEventHandler((object sender, MouseButtonEventArgs e) => ProcessItemClick(r.ID));
                    
                    Grid.SetRow(rect, y);
                    Grid.SetColumn(rect, 0);
                    Grid.SetColumnSpan(rect, grdRows.ColumnDefinitions.Count());
                    grdRows.Children.Add(rect);

                    y++;
                }
            }

            if(y == 0)
            {
                RowDefinition gridRowDef = new RowDefinition();
                gridRowDef.Height = new GridLength(ROWHEIGHT);
                grdRows.RowDefinitions.Add(gridRowDef);

                TextBlock cell = new TextBlock();
                cell.Text = LangResources.CurLang.NoDataToShow;
                cell.HorizontalAlignment = HorizontalAlignment.Center;
                cell.Style = Application.Current.FindResource("ListItem") as Style;

                Grid.SetRow(cell, 0);
                Grid.SetColumn(cell, 0);
                Grid.SetColumnSpan(cell, grdRows.ColumnDefinitions.Count());
                grdRows.Children.Add(cell);
            }

            scvRows.Content = grdRows;
            SelectedID = -1;
        }

        /// <summary>
        /// Highlight the requested item
        /// </summary>
        /// <param name="id">ID (not row number) of the row</param>
        public void HighlightItem(int id)
        {
            int OldID = SelectedID;
            SelectedID = id;

            if (SelectionMode == SelectMode.Highlight || SelectionMode == SelectMode.HighlightAndCallback)
            {
                if (OldID > -1)
                {
                    RowBackgrounds[OldID].Fill = DeselectedBackgroundBrush;
                    RowBackgrounds[OldID].Stroke = DeselectedBackgroundBrush;
                    RowBackgrounds[OldID].Opacity = DeselectedOpacity;
                }

                RowBackgrounds[SelectedID].Fill = SelectedBackgroundBrush;
                RowBackgrounds[SelectedID].Stroke = SelectedBackgroundBrush;
                RowBackgrounds[SelectedID].Opacity = SelectedOpacity;
            }
        }

        private void ProcessItemClick(int id)
        {
            if (SelectionMode == SelectMode.Highlight || SelectionMode == SelectMode.HighlightAndCallback) { 
                HighlightItem(id);
            } else
            {
                SelectedID = id;
            }

            if(SelectionMode == SelectMode.Callback || SelectionMode == SelectMode.HighlightAndCallback)
            {
                if(Callback_ItemClick != null)
                {
                    Callback_ItemClick();
                }
                else
                {
                    throw new Exception("Callback_ItemClick() is null");
                }
                
            }
            
        }

        public ListControl()
        {
            InitializeComponent();
        }
    }

    /// <summary>
    /// Definition of a column for the list
    /// </summary>
    public class ListColumn
    {
        public string Title { get; set; }
        public HorizontalAlignment Alignment { get; set; }
        public int Width { get; set; }

        public ListColumn()
        {
            this.Alignment = HorizontalAlignment.Left;
        }

        public ListColumn(string Title, int Width)
        {
            this.Title = Title;
            this.Width = Width;
            this.Alignment = HorizontalAlignment.Left;
        }

        public ListColumn(string Title, int Width, HorizontalAlignment Alignment)
        {
            this.Title = Title;
            this.Width = Width;
            this.Alignment = Alignment;
        }
    }

    /// <summary>
    /// Definition of a row, or a complete item
    /// </summary>
    public class ListRow
    {
        public int ID { get; set; }
        public List<object> ColumnData { get; set; }

        public ListRow()
        {

        }

        public ListRow(int ID, List<object> ColumnData)
        {
            this.ID = ID;
            this.ColumnData = ColumnData;
        }
    }
}
