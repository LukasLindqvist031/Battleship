using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Battleship
{
    public class SimpleMenu<T> : IMenu<T>
    {
        public readonly List<MenuItem<T>> MenuItems;
        public int SelectedIndex;
        public readonly int MenuTop;

        public SimpleMenu(List<MenuItem<T>> items)
        {
            MenuItems = items;
            SelectedIndex = 0;
            MenuTop = Console.CursorTop;
        }

        public void Draw()
        {
            Console.CursorVisible = false;

            for (int i = 0; i < MenuItems.Count; i++)
            {
                Console.SetCursorPosition(0, MenuTop + i);
                Console.Write(new string(' ', Console.WindowWidth));
            }

            for (int i = 0; i < MenuItems.Count; i++)
            {
                Console.SetCursorPosition(0, MenuTop + i);
                if (i == SelectedIndex)
                {
                    Console.WriteLine($"-> {MenuItems[i].Text.PadRight(10)}");
                }
                else
                {
                    Console.WriteLine($"   {MenuItems[i].Text.PadRight(10)}");
                }
            }
        }
        public void Up()
        {
            if (SelectedIndex > 0)
            {
                MenuItems[SelectedIndex].IsSelected = false;
                SelectedIndex--;
                MenuItems[SelectedIndex].IsSelected = true;
                Draw();
            }
        }
        public void Down()
        {
            if (SelectedIndex < MenuItems.Count - 1)
            {
                MenuItems[SelectedIndex].IsSelected = false;
                SelectedIndex++;
                MenuItems[SelectedIndex].IsSelected = true;
                Draw();
            }
        }
        public T GetSelectedItem()
        {
            return MenuItems[SelectedIndex].Value;
        }
    }
}
