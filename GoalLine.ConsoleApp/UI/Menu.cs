using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Text;
using System.Threading.Tasks;

namespace GoalLine.ConsoleApp.UI
{
    public class Menu
    {
        private const  int TopY = 4;
        private int CurrentOptionIndex = 0;
        private Dictionary<string, string> ExtraKeys = new Dictionary<string, string>();

        public bool RedrawOnRun { get; set; } = true;

        /// <summary>
        /// Allows the definition of an additional keypress, other than Enter/Esc
        /// </summary>
        /// <param name="Character">Single character to use for the option</param>
        /// <param name="Description">Description to show to the end user</param>
        public void AddExtraKeypress(string Character, string Description)
        {
            if(Character.Length != 1)
            {
                throw new ArgumentException("Single character only");
            }
            ExtraKeys.Add(Character.ToUpper(), Description);
        }

        List<MenuColumn> Columns = new List<MenuColumn>();
        List<MenuItem> Items = new List<MenuItem>();

        /// <summary>
        /// Add a column to the menu 
        /// </summary>
        /// <param name="col"></param>
        public void AddColumn(MenuColumn col)
        {
            Columns.Add(col);
        }

        /// <summary>
        /// Add an item to the list within the menu
        /// </summary>
        /// <param name="Item"></param>
        public void AddItem(MenuItem Item)
        {
            Items.Add(Item);
        }

        /// <summary>
        /// Operate the menu with the given columns, items and any extra keypresses.
        /// </summary>
        /// <returns>MenuReturnData containing the key pressed, and the ID of the last selected item</returns>
        public MenuReturnData RunMenu(string DefaultOptionID)
        {
            MenuReturnData retVal = new MenuReturnData();

            bool WasCursorVisible = Console.CursorVisible;
            Console.CursorVisible = false;

            DisplayHeaders();

            if(DefaultOptionID != "")
            {
                for (int i = 0; i < Items.Count; i++)
                {
                    if(Items[i].Id == DefaultOptionID)
                    {
                        CurrentOptionIndex = i;
                        break;
                    }
                }
            }

            for (int i = 0; i < Items.Count; i++)
            {
                DisplayItem(i, (i == CurrentOptionIndex));
            }

            Console.SetCursorPosition(0, Console.CursorTop + 2);
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.Write("[ENTER] to Select");
            foreach(KeyValuePair<string, string> k in ExtraKeys)
            {
                string KeyToDisplay = k.Key;
                if(KeyToDisplay == " ")
                {
                    KeyToDisplay = "SPACE";
                }
                Console.Write(String.Format(", [{0}] to {1}", KeyToDisplay, k.Value));
            }

            bool bored = false;
            while(!bored)
            {
                ConsoleKeyInfo k = Console.ReadKey(true);

                retVal.ItemID = Items[CurrentOptionIndex].Id;

                switch(k.Key)
                {
                    case ConsoleKey.Escape:
                        retVal.Keypress = MenuKeypressConstants.ESC;
                        bored = true;
                        break;

                    case ConsoleKey.Enter:
                        retVal.Keypress = MenuKeypressConstants.ENTER;
                        bored = true;
                        break;

                    case ConsoleKey.UpArrow:
                        if (CurrentOptionIndex > 0)
                        {
                            DisplayItem(CurrentOptionIndex, false);
                            CurrentOptionIndex--;
                            DisplayItem(CurrentOptionIndex, true);
                        }
                        break;

                    case ConsoleKey.DownArrow:
                        if (CurrentOptionIndex < Items.Count - 1)
                        {
                            DisplayItem(CurrentOptionIndex, false);
                            CurrentOptionIndex++;
                            DisplayItem(CurrentOptionIndex, true);
                        }
                        break;

                    default:
                        string KChar = k.KeyChar.ToString().ToUpper();
                        foreach(KeyValuePair<string, string> kvp in ExtraKeys)
                        {
                            if(kvp.Key == KChar)
                            {
                                retVal.Keypress = KChar;
                                bored = true;
                            }
                        }
                        break;

                }
            }

            Console.CursorVisible = WasCursorVisible;

            return retVal;
        }

        public MenuReturnData RunMenu()
        {
            return RunMenu("");
        }

        /// <summary>
        /// Redraw the given menu item in it's selected or deselected state
        /// </summary>
        /// <param name="Index">Item number (order, not ID)</param>
        /// <param name="Selected">true if the item is to show as selected</param>
        private void DisplayItem(int Index, bool Selected)
        {
            int y = TopY + 1 + Index;
            int x = 1;
            int c = 0;

            ConsoleColor CurBack = Console.BackgroundColor;

            Console.SetCursorPosition(0, y);
            if(Selected)
            {
                Console.BackgroundColor = ConsoleColor.Gray;
            } else
            {
                Console.BackgroundColor = ConsoleColor.Black;
            }
            Console.Write(new string(Convert.ToChar(" "), Console.WindowWidth - 1));

            MenuItem ThisItem = Items[Index];
            foreach(MenuColumn col in Columns)
            {
                Console.ForegroundColor = col.Colour;

                string DisplayText = ThisItem.DisplayText[c] + new string(Convert.ToChar(" "), col.Width);
                DisplayText = DisplayText.Substring(0, col.Width);

                Console.SetCursorPosition(x, y);
                Console.Write(DisplayText);
                x += col.Width;
                c++;
            }

            Console.BackgroundColor = CurBack;
        }

        /// <summary>
        /// Show the headers for each column in the menu
        /// </summary>
        private void DisplayHeaders()
        {
            int x = 1;
            Console.ForegroundColor = ConsoleColor.Gray;

            foreach (MenuColumn col in Columns)
            {
                string DisplayText = col.Title + new string(Convert.ToChar(" "), col.Width);
                DisplayText = DisplayText.Substring(0, col.Width);

                Console.SetCursorPosition(x, TopY);
                Console.Write(col.Title);
                x += col.Width;
            }
        }

        public void UpdateItemText(string ItemID, int ColumnID, string Text)
        {
            for(int i = 0; i < Items.Count(); i++)
            {
                if(Items[i].Id == ItemID)
                {
                    Items[i].DisplayText[ColumnID] = Text;
                    DisplayItem(i, i == CurrentOptionIndex);
                    break;
                }
            }
        }
    }
}
