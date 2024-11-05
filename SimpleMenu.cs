using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Battleship
{
    public class SimpleMenu<T> : IMenu<T>
    {
        private readonly List<MenuItem<T>> MenuItems;
        private int SelectedIndex;
        private readonly int MenuTop;
        private readonly int MenuLeft;
        private readonly bool BelowGrid;

        public SimpleMenu(List<MenuItem<T>> items, bool belowGrid = false, int? specificY = null)
        {
            MenuItems = items;
            SelectedIndex = 0;
            BelowGrid = belowGrid;

            int menuWidth = items.Max(item => item.Text.Length) + 4; 
            int menuHeight = items.Count;

            if (specificY.HasValue)
            {
                MenuTop = specificY.Value;
                MenuLeft = TextPresentation.GetCenterX(menuWidth);
            }
            else if (belowGrid)
            {
                MenuTop = (Console.WindowHeight / 2) + 6;
                MenuLeft = TextPresentation.GetCenterX(menuWidth * 2) - menuWidth * 2 + 2; 
            }
            else
            {
                MenuTop = TextPresentation.GetCenterY(menuHeight);
                MenuLeft = TextPresentation.GetCenterX(menuWidth);
            }
        }

        public void Draw()
        {
            Console.CursorVisible = false;

            for (int i = 0; i < MenuItems.Count; i++)
            {
                Console.SetCursorPosition(MenuLeft, MenuTop + i);
                Console.Write(new string(' ', MenuItems[i].Text.Length + 4));
            }

            for (int i = 0; i < MenuItems.Count; i++)
            {
                Console.SetCursorPosition(MenuLeft, MenuTop + i);
                if (i == SelectedIndex)
                {
                    Console.Write($"-> {MenuItems[i].Text}");
                }
                else
                {
                    Console.Write($"   {MenuItems[i].Text}");
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
