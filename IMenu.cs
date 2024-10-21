using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Battleship
{
    public interface IMenu<T>
    {
        void Draw();
        void up();
        void down();

        T GetSelectedItem();
    }
}
