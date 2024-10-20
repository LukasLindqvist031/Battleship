using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Battleship
{
    public class Human : Player
    {
        public Human(string name, Grid grid, List<Ship> ships)
            : base(name, grid, ships)
        {
        }
    }
}
