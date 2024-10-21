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
        protected List<IPlayerAction> Actions { get; private set; }
        protected IShootingStrategy ShootingStrategy;


        protected Player(string name, Grid grid, List<Ship> ships, List<IPlayerAction> actions, IShootingStrategy shootingStrategy)
        {
            Name = name;
            PlayerGrid = grid;
            Ships = ships;
            Actions = actions;
            ShootingStrategy = shootingStrategy;   
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
        public void PerformAction(int actionIndex)
        {
            if (actionIndex >= 0 && actionIndex < Actions.Count)
            {
                Actions[actionIndex].Execute(this); // Execute the chosen action
            }
            else
            {
                throw new ArgumentOutOfRangeException("Invalid action index");
            }
        }

        public void PerformShooting()
        {
            ShootingStrategy.Shoot(this); // Use the injected shooting strategy
        }
    }
}
