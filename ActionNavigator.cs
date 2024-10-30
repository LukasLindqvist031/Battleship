﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Battleship
{
    public class ActionNavigator<T>
    {
        private readonly SimpleMenu<T> _menu;

        public ActionNavigator(SimpleMenu<T> menu)
        {
            _menu = menu;
        }

        public T Navigate()
        {
            while (true)
            {
                _menu.Draw();
                var input = Console.ReadKey(true);
                switch (input.Key)
                {
                    case ConsoleKey.UpArrow:
                        _menu.Up();
                        break;
                    case ConsoleKey.DownArrow:
                        _menu.Down();
                        break;
                    case ConsoleKey.Enter:
                        return _menu.GetSelectedItem();
                    default:
                        Console.WriteLine("Invalid key. Use arrows to navigate and Enter to select.");
                        break;
                }
            }
        }
    }
}