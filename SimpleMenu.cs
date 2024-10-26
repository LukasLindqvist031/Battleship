using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Battleship
{
    public class SimpleMenu<T>
    {
        private readonly List<MenuItem<T>> _menuItems;
        private int _selectedIndex;
        private readonly int menuTop;

        public SimpleMenu(List<MenuItem<T>> items)
        {
            _menuItems = items;
            _selectedIndex = 0;
            menuTop = Console.CursorTop;
            Console.CursorVisible = false; //Denna kod gör kanske resterande cursorposition onödig. Kontrollera! 
            if (_menuItems.Any())
            {
                _menuItems[0].IsSelected = true;
            }
        }

        public void Draw()
        {
            //Console.SetCursorPosition(0, menuTop);

            // Clear the menu 
            for (int i = 0; i < _menuItems.Count; i++)
            {
                Console.WriteLine(new string(' ', Console.WindowWidth));
            }

            // Move cursor back to start
            Console.SetCursorPosition(0, menuTop);

            for (int i = 0; i < _menuItems.Count; i++)
            {
                var item = _menuItems[i];

                // Fundera på om detta är värt det. Eller om det finns rimligare lösningar. Jämför med Pokemon spelet. 
                if (_selectedIndex == i)
                {
                    Console.WriteLine($"-> {item.Text.PadRight(10)}"); // Highlighted option
                }
                else
                {
                    Console.WriteLine($"   {item.Text.PadRight(10)}"); // Non-highlighted option with spaces for alignment
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
