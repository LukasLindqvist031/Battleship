using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Battleship
{
    public class SimpleMenu<T>
    {
        public readonly List<MenuItem<T>> _menuItems;
        public int _selectedIndex;
        public readonly int menuTop;

        public SimpleMenu(List<MenuItem<T>> items)
        {
            _menuItems = items;
            _selectedIndex = 0;
            menuTop = Console.CursorTop;
        }

        public void Draw()
        {
            Console.CursorVisible = false;

            // Clear only the lines used by the menu
            for (int i = 0; i < _menuItems.Count; i++)
            {
                Console.SetCursorPosition(0, menuTop + i);
                Console.Write(new string(' ', Console.WindowWidth));
            }

            // Draw menu with arrow for selected item
            for (int i = 0; i < _menuItems.Count; i++)
            {
                Console.SetCursorPosition(0, menuTop + i);
                if (i == _selectedIndex)
                {
                    Console.WriteLine($"-> {_menuItems[i].Text.PadRight(10)}");
                }
                else
                {
                    Console.WriteLine($"   {_menuItems[i].Text.PadRight(10)}");
                }
            }
        }

        public void Up()
        {
            if (_selectedIndex > 0)
            {
                _menuItems[_selectedIndex].IsSelected = false;
                _selectedIndex--;
                _menuItems[_selectedIndex].IsSelected = true;
                Draw();
            }
        }

        public void Down()
        {
            if (_selectedIndex < _menuItems.Count - 1)
            {
                _menuItems[_selectedIndex].IsSelected = false;
                _selectedIndex++;
                _menuItems[_selectedIndex].IsSelected = true;
                Draw();
            }
        }
        public MenuItem<T> GetSelectedItem()
        {
            return _menuItems[_selectedIndex];
        }
    }
}
