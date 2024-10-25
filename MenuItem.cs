using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Battleship
{
    public class MenuItem<T>
    {
        public string Text { get; }
        public T Value { get; }
        public bool IsSelected { get; set; }

        public MenuItem(string text, T value)
        {
            Text = text;
            Value = value;
            IsSelected = false;
        }
    }
}
