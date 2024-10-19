using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Battleship
{
    public class ShipPlacementService : IShipPlacement
    {
        private Random _random = new Random();

        public bool PlaceShipRandomly(Grid grid, Ship ship)
        {
            bool placed = false;

            while (!placed)
            {

            }
        }
    }
}
