using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Battleship
{
    public class Cell
    {
        public int Row { get; set; }
        public int Column { get; set; }
        public Ship Ship { get; set; } 
        public bool IsHit { get; set; } 
        public bool WasRepaired { get; set; } = false;

        public Cell(int row, int column)
        {
            Row = row;
            Column = column;
            IsHit = false;
        }

        public bool IsEmpty() 
        {
            return Ship == null;
        }

        public bool HasShip() {  return Ship != null; }
    }
}