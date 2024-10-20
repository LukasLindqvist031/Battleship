using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Battleship
{
    public class Computer : Player
    {
        public Computer(string name, Grid grid, List<Ship> ships)
            : base(name, grid, ships)
        {
        }
    }
}
