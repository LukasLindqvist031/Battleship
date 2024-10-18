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

        public Cell(int row, int column) { Row = row; Column = column; }
        public bool IsHit { get; set; } //Checks to see if the cell has been hit.
        public Ship Ship { get; set; } //Refers to a ship if there is one.

        public bool IsEmpty() //Method to check if the cell is empty (no ship).
        {
            return Ship == null;
        }
    }
}