using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Battleship
{
    abstract class Player
    {
        public string Name { get; set; }
        public Grid PlayerGrid { get; set; }
        public List<Ship> Ships { get; set; }

        public Player(string name)
        {
            Name = name;
            PlayerGrid = new Grid();
            Ships = new List<Ship>();
        }

        public abstract void TakeAction(IPlayerAction action);

        public bool AreAllShipsSunk()
        {
            foreach (var ship in Ships)
            {
                if (!ship.IsSunk())
                {
                    return false; 
                }
            }
            return true; 
        }
    }
}
