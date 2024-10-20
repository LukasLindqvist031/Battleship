using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Battleship
{
    public abstract class Player 
    {
        public string Name { get; set; }
        public Grid PlayerGrid { get; set; }
        public List<Ship> Ships { get; set; }

        protected Player(string name, Grid grid, List<Ship> ships)
        {
            Name = name;
            PlayerGrid = grid;
            Ships = ships;
        }

        public void RemoveSunkShip(Ship ship)
        {
            if (Ships.Contains(ship)) {  Ships.Remove(ship); }
        }

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
        public void TakeAction<T>(IPlayerAction<T> action) where T : Player
        {
            action.Execute((T)this); 
        }
    }
}
