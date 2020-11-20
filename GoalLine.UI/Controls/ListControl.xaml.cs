using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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
    /// Interaction logic for ListControl.xaml
    /// </summary>
    public partial class ListControl : UserControl
    {
        public event EventHandler RowClicked;
        public string Title { get; set; }

        public List<ListColumn> Columns { get; set; }
        public List<ListRow> Rows { get; set; }

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

        public void ItemClickedEventHandler(object sender, RoutedEventArgs e)
        {
            TextBlock t = (TextBlock)sender;
            SelectedID = (int)t.Tag;

            ListRow selected = (from r in Rows
                                where r.ID == SelectedID
                                select r).First();


            RowClicked.Invoke(selected, e);
        }

        public void Render()
        {
            const bool GridLines = true;
            int x;
            int y;

            lblCaption.Text = Title;
            grdMain.ShowGridLines = GridLines;


            // Initialise Grids
            if(grdHeader != null)
            {
                grdMain.Children.Remove((UIElement)this.FindName("grdHeader"));
                grdMain.Children.Remove((UIElement)this.FindName("grdRows"));
            }
            grdHeader = new Grid();
            grdHeader.Name = "grdHeader";

            grdRows = new Grid();
            grdRows.Name = "grdRows";

            grdHeader.Height = 30;
            grdHeader.ShowGridLines = GridLines;

            grdRows.ShowGridLines = GridLines;

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
                    } else
                    {
                        grdRows.ColumnDefinitions.Add(h);
                    }
                }
            }

            foreach (ListRow r in Rows)
            {
                RowDefinition gridRowDef = new RowDefinition();
                gridRowDef.Height = new GridLength(25);
                grdRows.RowDefinitions.Add(gridRowDef);
            }

            // Render Column Headers
            x = 0;
            foreach (ListColumn c in Columns)
            {
                TextBlock heading = new TextBlock();
                heading.Text = c.Title;
                //heading.Width = c.Width;
                heading.HorizontalAlignment = c.Alignment;

                //Grid.SetRow(heading, 1);
                Grid.SetColumn(heading, x);
                grdHeader.Children.Add(heading);

                x++;
            }
            Grid.SetRow(grdHeader, 1);
            grdMain.Children.Add(grdHeader);

            // Render rows
            y = 0;
            foreach(ListRow r in Rows)
            {
                x = 0;

                foreach (string s in r.ColumnData)
                {
                    TextBlock cell = new TextBlock();
                    cell.Text = s;
                    cell.HorizontalAlignment = Columns[x].Alignment;

                    cell.Tag = r.ID;
                    cell.MouseLeftButtonUp += new MouseButtonEventHandler(ItemClickedEventHandler);

                    Grid.SetRow(cell, y);
                    Grid.SetColumn(cell, x);
                    grdRows.Children.Add(cell);

                    x++;
                }

                y++;
            }
            //Grid.SetRow(grdRows, 2);
            scvRows.Content = grdRows;

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
        public List<string> ColumnData { get; set; }

        public ListRow()
        {

        }

        public ListRow(int ID, List<string> ColumnData)
        {
            this.ID = ID;
            this.ColumnData = ColumnData;
        }

        public ListRow(int ID, string FirstCellData)
        {
            List<string> l = new List<string>();
            l.Add(FirstCellData);

            this.ID = ID;
            this.ColumnData = l;
        }
    }
}
