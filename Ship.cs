using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Battleship
{
    public class Ship
    {
        public int Length { get; set; }
        public List<Cell> PlacedOnCell { get; set; }
        public int HitTaken { get; set; }

        public Ship(int length)
        {
            Length = length;
            PlacedOnCell = new List<Cell>();
            HitTaken = 0; 
        }

        public bool IsSunk()
        {
            return HitTaken >= Length;
        }

        public void TakeHit()
        {
            HitTaken++;
        }
        public Ship Clone()
        {
            return new Ship(Length);
        }
    }
}
